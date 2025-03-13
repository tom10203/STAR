using KinematicCharacterController;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CrouchInput
{
    None, Toggle
}

public enum Stance
{
    Stand, Crouch, Slide
}

public struct CharacterState
{
    public bool grounded;
    public Stance stance;
    public Vector3 velocity;
    public Vector3 acceleration;
}
public struct CharacterInput
{
    public Quaternion rotation;
    public Vector2 move;
    public bool jump;
    public CrouchInput crouch;
    public bool jumpSustain;
}
public class PlayerCharacter : MonoBehaviour, ICharacterController
{
    [SerializeField] KinematicCharacterMotor motor;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform root;
    [SerializeField] float walkSpeed = 20f;
    [SerializeField] float crouchSpeed = 7f;
    [SerializeField] private float walkResponse = 25f;
    [SerializeField] private float crouchResponse = 20f;
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float jumpSpeedAdjustment = 2f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float coyoteTimeSlide = 0.2f;
    [SerializeField] private float airSpeed = 15f;
    [SerializeField] private float airAcceleration = 70f;
    [SerializeField] private float gravity = -90f;
    [Space]
    [SerializeField] private float slideStartSpeed = 25f;
    [SerializeField] private float slideStartSpeedFromAir = 35f;
    [SerializeField] private float slideStartSpeedFromAirAdjustment = 1.5f;
    [SerializeField] private float slideEndSpeed = 15f;
    [SerializeField] private float slideFriction = 0.8f;
    [SerializeField] private float slideFrictionAdjustment = 0f;
    [SerializeField] private float slideSteerAcceleration = 5f;
    [SerializeField] private float slideGravity = -90f;
    [Range(0,1)]    
    [SerializeField] private float jumpSustainGravity = 0.4f;
    
    [SerializeField] float standHeight = 1f;
    [SerializeField] float crouchHeight = 0.5f;
    [SerializeField] private float crouchHeightResponse = 15f;
    [Range(0, 1)]
    [SerializeField] float standCameraHeight = 0.9f;
    [Range(0, 1)]
    [SerializeField] float crouchCameraHeight = 0.7f;

    private CharacterState _state;
    private CharacterState _lastState;
    private CharacterState _tempState;

    private Quaternion _requestedRotation;
    private Vector3 _requestedMovement;
    private bool _requestedJump;
    private bool _requestedSustainedJump;
    private bool _requestedCrouch;
    private bool _requestedCrouchInAir;
    private float _timeSinceUngrounded;
    private float _timeSinceJumpRequest;
    private bool _ungroundedDueToJump;
    private float _timeSinceLanded;
    private bool _increaseTimeSinceLanded;
    private Collider[] _uncrouchOverlapResults;

    public int health = 100;

    Vector3 _storedGroundNormal;
    int _storedGroundLayer = 0;


    [SerializeField] private TMP_Text healthText;

    private PlayerInput playerInput;
    private PlayerShooting playerShooting;
    public GameObject levelFailedDied, crossHair;

    private InGameUI inGameUI;

    public void Initialise()
    {
        motor.CharacterController = this;
        _state.stance = Stance.Stand;
        _lastState = _state;
        _uncrouchOverlapResults = new Collider[8];

        _storedGroundNormal = motor.CharacterUp;
    }

    public void UpdateInput(CharacterInput input)
    {
        _requestedRotation = input.rotation;
        _requestedMovement = new Vector3 (input.move.x, 0, input.move.y);
        _requestedMovement = Vector3.ClampMagnitude(_requestedMovement, 1f);
        _requestedMovement = input.rotation * _requestedMovement;
        var wasRequestingJump = _requestedJump;
        _requestedJump = _requestedJump || input.jump;
        if (_requestedJump && !wasRequestingJump)
        {
            _timeSinceJumpRequest = 0f;
        }
        _requestedSustainedJump = input.jumpSustain;
        var wasRequestingCrouch = _requestedCrouch;
        _requestedCrouch = input.crouch switch
        {
            CrouchInput.Toggle => !_requestedCrouch,
            CrouchInput.None => _requestedCrouch,
            _ => _requestedCrouch
        };
        if (_requestedCrouch && !wasRequestingCrouch)
        {
            _requestedCrouchInAir = !_state.grounded;
        }
        else if (!_requestedCrouch && wasRequestingCrouch)
        {
            _requestedCrouchInAir = false;
        }

    }

    public void UpdateBody(float deltaTime)
    {
        var currentHeight = motor.Capsule.height;
        var normalisedHeight = currentHeight / standHeight;
        var cameraTargetHeight = currentHeight *
        (
            _state.stance is Stance.Stand ? standCameraHeight : crouchCameraHeight
        );
        var rootTargetScale = new Vector3(1f, normalisedHeight, 1f);

        cameraTarget.localPosition = Vector3.Lerp(cameraTarget.localPosition, new Vector3(0f, cameraTargetHeight, 0f), 1f - Mathf.Exp(-crouchHeightResponse * deltaTime));

        root.localScale = Vector3.Lerp(root.localScale, rootTargetScale, 1f - Mathf.Exp(-crouchHeightResponse * deltaTime));
    }

    public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) 
    {
        var forward = Vector3.ProjectOnPlane(_requestedRotation * Vector3.up, motor.CharacterUp);

        if (forward != Vector3.zero)
        {
            currentRotation = Quaternion.LookRotation(forward, motor.CharacterUp);
        }

        //currentRotation = _requestedRotation;
    }
    /// <summary>
    /// This is called when the motor wants to know what its velocity should be right now
    /// </summary>
    public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) 
    {
        _state.acceleration = Vector3.zero;

        if (motor.GroundingStatus.IsStableOnGround)
        {
            _timeSinceUngrounded = 0f;
            _ungroundedDueToJump = false;
            _storedGroundNormal = motor.GroundingStatus.GroundNormal;
            _storedGroundLayer = motor.GroundingStatus.GroundCollider.gameObject.layer;

            var groundedMovement = motor.GetDirectionTangentToSurface
            (
                direction: _requestedMovement,
                surfaceNormal: motor.GroundingStatus.GroundNormal
            ) * _requestedMovement.magnitude;


            {
                var moving = groundedMovement.sqrMagnitude > 0f;
                var crouching = _state.stance is Stance.Crouch;
                var wasStanding = _lastState.stance is Stance.Stand;
                var wasInAir = !_lastState.grounded;

                if (wasInAir)
                {
                    _increaseTimeSinceLanded = true;
                    //_timeSinceLanded += deltaTime;
                }

                if (_increaseTimeSinceLanded)
                {
                    _timeSinceLanded += deltaTime;
                }

                if (moving && crouching && (wasStanding || wasInAir))
                {
                    _state.stance = Stance.Slide;

                    var effectiveSlideSpeed = _timeSinceLanded < coyoteTimeSlide ? slideStartSpeedFromAir: slideStartSpeed;
                    effectiveSlideSpeed = _storedGroundLayer == 11 ? effectiveSlideSpeed * slideStartSpeedFromAirAdjustment : effectiveSlideSpeed;

                    if (wasInAir)
                    {
                        currentVelocity = Vector3.ProjectOnPlane
                            (
                                vector: _lastState.velocity,
                                planeNormal: motor.GroundingStatus.GroundNormal
                            );

                    }

                    
                    if (!_lastState.grounded && !_requestedCrouchInAir)
                    {
                        effectiveSlideSpeed = 0f;
                        _requestedCrouchInAir = false;
                    }
                    var slideSpeed = Mathf.Max(effectiveSlideSpeed, currentVelocity.magnitude);
                    currentVelocity = motor.GetDirectionTangentToSurface
                    (
                        direction: currentVelocity,
                        surfaceNormal: motor.GroundingStatus.GroundNormal
                    ) * slideSpeed;
                }
            } 
            if (_state.stance is Stance.Stand or Stance.Crouch)
            {
                

                var speed = _state.stance is Stance.Stand ? walkSpeed : crouchSpeed;
                var response = _state.stance is Stance.Stand ? walkResponse : crouchResponse;

                var targetVelocity = groundedMovement * speed;

                var moveVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 1f - Mathf.Exp(-response * deltaTime));
                _state.acceleration = moveVelocity - currentVelocity;
                currentVelocity = moveVelocity;
            }
            else
            {
                var effectiveSlideFriction = _storedGroundLayer == 11 ? slideFriction *  slideFrictionAdjustment : slideFriction;
                currentVelocity -= currentVelocity * (effectiveSlideFriction * deltaTime);

                {
                    var force = Vector3.ProjectOnPlane
                    (
                        vector: -motor.CharacterUp,
                        planeNormal: motor.GroundingStatus.GroundNormal
                    ) * slideGravity;

                    currentVelocity -= force * deltaTime;
                }

                {
                    var currentSpeed = currentVelocity.magnitude;
                    var targetVelocity = groundedMovement * currentSpeed;
                    var steerVelocity = currentVelocity;
                    var steeringForce = (targetVelocity - steerVelocity) * slideSteerAcceleration * deltaTime;
                    steerVelocity += steeringForce;
                    steerVelocity = Vector3.ClampMagnitude(steerVelocity, currentSpeed);

                    _state.acceleration = (steerVelocity - currentVelocity)/ deltaTime;

                    currentVelocity = steerVelocity;
                }
                if (currentVelocity.magnitude < slideEndSpeed)
                {
                    _state.stance = Stance.Crouch;
                }
            }
        }
        else
        {
            _increaseTimeSinceLanded = false;
            _timeSinceLanded = 0f;
            _timeSinceUngrounded += deltaTime;

            if (_requestedMovement.sqrMagnitude > 0f)
            {
                var planarMovement = Vector3.ProjectOnPlane
                (
                    vector: _requestedMovement,
                    planeNormal: _storedGroundNormal
                    //planeNormal: motor.CharacterUp
                ) * _requestedMovement.magnitude;

                var currentPlanarVelocity = Vector3.ProjectOnPlane
                (
                    vector: currentVelocity,
                    planeNormal: _storedGroundNormal
                //planeNormal: motor.CharacterUp
                );

                var movementForce = planarMovement * airAcceleration * deltaTime;

                if (currentPlanarVelocity.magnitude < airSpeed)
                {
                    var targetPlanarVelocity = currentPlanarVelocity + movementForce;

                    targetPlanarVelocity = Vector3.ClampMagnitude(targetPlanarVelocity, airSpeed);

                    movementForce = targetPlanarVelocity - currentPlanarVelocity;
                }

                else if (Vector3.Dot(currentPlanarVelocity, movementForce) > 0f)
                {
                    var constrainedMovementForce = Vector3.ProjectOnPlane
                    (
                        vector: movementForce,
                        planeNormal: currentPlanarVelocity.normalized
                    );

                    movementForce = constrainedMovementForce;
                }

                if (motor.GroundingStatus.FoundAnyGround)
                {
                    if (Vector3.Dot(movementForce, currentVelocity + movementForce) > 0f)
                    {
                        var obstructionNormal = Vector3.Cross
                        (
                            motor.CharacterUp,
                            Vector3.Cross
                            (
                                motor.CharacterUp,
                                motor.GroundingStatus.GroundNormal
                            )
                        ).normalized;

                        movementForce = Vector3.ProjectOnPlane(movementForce, obstructionNormal);
                    }

                }

                currentVelocity += movementForce;

            }
            var effectiveGravity = gravity;
            //var verticalSpeed = Vector3.Dot(motor.CharacterUp, currentVelocity);
            var verticalSpeed = Vector3.Dot(_storedGroundNormal, currentVelocity);
            if (_requestedSustainedJump && verticalSpeed > 0f)
            {
                effectiveGravity *= jumpSustainGravity;
            }
            //currentVelocity += motor.CharacterUp * effectiveGravity * deltaTime;
            currentVelocity += _storedGroundNormal * effectiveGravity * deltaTime;
        }


        if (_requestedJump)
        {
            var grounded = motor.GroundingStatus.IsStableOnGround;
            var canCoyoteJump = _timeSinceUngrounded < coyoteTime && !_ungroundedDueToJump;

            if (grounded || canCoyoteJump)
            {
                _requestedJump = false;
                _requestedCrouch = false;
                _requestedCrouchInAir = false;

                motor.ForceUnground(time: 0f);
                _ungroundedDueToJump = true;

                var currentVerticalSpeed = Vector3.Dot(currentVelocity, _storedGroundNormal);
                //var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);

                var effectiveJumpSpeed = _storedGroundLayer == 11 ? jumpSpeed * jumpSpeedAdjustment : jumpSpeed;
                var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, effectiveJumpSpeed);
                //currentVelocity += motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);
                currentVelocity += _storedGroundNormal * (targetVerticalSpeed - currentVerticalSpeed);
            }
            else
            {
                _timeSinceJumpRequest += deltaTime;

                var canJumpLater = _timeSinceJumpRequest < coyoteTime;
                _requestedJump = canJumpLater;
            }

        }
    }
    /// <summary>
    /// This is called before the motor does anything
    /// </summary>
    public void BeforeCharacterUpdate(float deltaTime) 
    {
        _tempState = _state;
        if (_requestedCrouch && _state.stance is Stance.Stand)
        {
            _state.stance = Stance.Crouch;
            motor.SetCapsuleDimensions
            (
                radius: motor.Capsule.radius,
                height: crouchHeight,
                yOffset: crouchHeight * 0.5f
            );
        }
    }
    /// <summary>
    /// This is called after the motor has finished its ground probing, but before PhysicsMover/Velocity/etc.... handling
    /// </summary>
    public void PostGroundingUpdate(float deltaTime) 
    {
        if (!motor.GroundingStatus.IsStableOnGround && _state.stance is Stance.Stand)
        {
            _state.stance = Stance.Crouch;
        }
    }
    /// <summary>
    /// This is called after the motor has finished everything in its update
    /// </summary>
    public void AfterCharacterUpdate(float deltaTime) 
    {
        if (!_requestedCrouch && _state.stance is not Stance.Stand)
        {
            _state.stance = Stance.Stand;
            motor.SetCapsuleDimensions
            (
                radius: motor.Capsule.radius,
                height: standHeight,
                yOffset: standHeight * 0.5f
            );

            if (motor.CharacterOverlap(motor.TransientPosition, motor.TransientRotation, _uncrouchOverlapResults, motor.CollidableLayers, QueryTriggerInteraction.Ignore) > 0)
            {
                _requestedCrouch = true;
                motor.SetCapsuleDimensions
                (
                    radius: motor.Capsule.radius,
                    height: crouchHeight,
                    yOffset: crouchHeight * 0.5f
                );
            }
            else
            {
                _state.stance = Stance.Stand;
            }
        }

        _state.grounded = motor.GroundingStatus.IsStableOnGround;
        _state.velocity = motor.Velocity;
        _lastState = _tempState;
    }
    /// <summary>
    /// This is called after when the motor wants to know if the collider can be collided with (or if we just go through it)
    /// </summary>
    public bool IsColliderValidForCollisions(Collider coll) => true;
    /// <summary>
    /// This is called when the motor's ground probing detects a ground hit
    /// </summary>
    public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
    /// <summary>
    /// This is called when the motor's movement logic detects a hit
    /// </summary>
    public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }
    /// <summary>
    /// This is called after every move hit, to give you an opportunity to modify the HitStabilityReport to your liking
    /// </summary>
    public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }
    /// <summary>
    /// This is called when the character detects discrete collisions (collisions that don't result from the motor's capsuleCasts when moving)
    /// </summary>
    public void OnDiscreteCollisionDetected(Collider hitCollider) { }
    public Transform GetCameraTarget() => cameraTarget;

    public void SetPosition(Vector3 position, bool killVelocity = true) 
    {
        motor.SetPosition(position);
        if (killVelocity)
        {
            motor.BaseVelocity = Vector3.zero;
        }
    }

    public CharacterState GetState() => _state;
    public CharacterState GetLastState() => _lastState;

    public void TakeDamage(int damage)
    {
        //Debug.Log("Player got hit for " + damage + " damage");
        health -= damage;

        if (healthText != null)
        {
            healthText.text = "HEALTH: " + health;
        }

        if (health <= 0)
        {
            playerInput.enabled = false;
            playerShooting.enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (crossHair != null)
            {
                crossHair.SetActive(false);
            }
            if (levelFailedDied != null)
            {
                levelFailedDied.SetActive(true);
            }
            inGameUI.StopTimer();
        }
    }

}
