using KinematicCharacterController;
using UnityEngine;

public enum CrouchInput
{
    None, Toggle
}

public enum Stance
{
    Stand, Crouch
}
public struct CharacterInput
{
    public Quaternion rotation;
    public Vector2 move;
    public bool jump;
    public CrouchInput crouch;
}
public class PlayerCharacter : MonoBehaviour, ICharacterController
{
    [SerializeField] KinematicCharacterMotor motor;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] float walkSpeed = 20f;
    [SerializeField] private float jumpSpeed = 20f;
    [SerializeField] private float gravity = -90f;
    [SerializeField] float crouchSpeed = 7f;
    [SerializeField] float standHeight = 2f;
    [SerializeField] float crouchHeight = 1f;
    [Range(0, 1)]
    [SerializeField] float standCameraHeight = 0.9f;
    [Range(0, 1)]
    [SerializeField] float crouchCameraHeight = 0.7f;


    private Quaternion _requestedRotation;
    private Vector3 _requestedMovement;
    private bool _requestedJump;
    private bool _requestedCrouch;
    private Stance _stance;
    public void Initialise()
    {
        motor.CharacterController = this;
        _stance = Stance.Stand;
    }

    public void UpdateInput(CharacterInput input)
    {
        _requestedRotation = input.rotation;
        _requestedMovement = new Vector3 (input.move.x, 0, input.move.y);
        _requestedMovement = Vector3.ClampMagnitude(_requestedMovement, 1f);
        _requestedMovement = input.rotation * _requestedMovement;
        _requestedJump = _requestedJump || input.jump;
        _requestedCrouch = input.crouch switch
        {
            CrouchInput.Toggle => !_requestedCrouch,
            CrouchInput.None => _requestedCrouch,
            _ => _requestedCrouch
        };

    }

    public void UpdateBody()
    {
        var currentHeight = motor.Capsule.height;
        var cameraTargetHeight = currentHeight *
        (
            _stance is Stance.Stand ? standCameraHeight : crouchCameraHeight
        );

        cameraTarget.localPosition = new Vector3(0f, cameraTargetHeight, 0f);
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
        if (motor.GroundingStatus.IsStableOnGround)
        {
            var groundedMovement = motor.GetDirectionTangentToSurface
            (
                direction: _requestedMovement,
                surfaceNormal: motor.GroundingStatus.GroundNormal
            ) * _requestedMovement.magnitude;

            var speed = _stance is not Stance.Stand ? walkSpeed: crouchSpeed;

            currentVelocity = groundedMovement * speed;
        }
        else
        {
            currentVelocity += motor.CharacterUp * gravity * deltaTime;
        }


        if (_requestedJump)
        {
            _requestedJump = false;

            motor.ForceUnground(time: 0f);

            var currentVerticalSpeed = Vector3.Dot(currentVelocity, motor.CharacterUp);
            var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, jumpSpeed);

            currentVelocity += motor.CharacterUp * (targetVerticalSpeed - currentVerticalSpeed);


        }
    }
    /// <summary>
    /// This is called before the motor does anything
    /// </summary>
    public void BeforeCharacterUpdate(float deltaTime) 
    {
        if (_requestedCrouch && _stance is Stance.Stand)
        {
            _stance = Stance.Crouch;
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
    public void PostGroundingUpdate(float deltaTime) { }
    /// <summary>
    /// This is called after the motor has finished everything in its update
    /// </summary>
    public void AfterCharacterUpdate(float deltaTime) 
    {
        if (!_requestedCrouch && _stance is not Stance.Stand)
        {
            _stance = Stance.Stand;
            motor.SetCapsuleDimensions
            (
                radius: motor.Capsule.radius,
                height: standHeight,
                yOffset: standHeight * 0.5f
            );
        }
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
}
