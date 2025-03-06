using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    [SerializeField] private PlayerCharacter2 playerCharacter;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private CameraSpring cameraSpring;
    [SerializeField] private CameraLean cameraLean;

    [SerializeField] private PlayerInput _input;
    InputAction _move;
    InputAction _look;
    InputAction _jump;
    InputAction _crouch;

    private void Start()
    {
        playerCharacter.Initialise();
        playerCamera.Initialise(playerCharacter.GetCameraTarget());
        cameraSpring.Initialise();
        cameraLean.Initialise();


        _move = _input.actions.FindAction("Move");
        _look = _input.actions.FindAction("Look");
        _jump = _input.actions.FindAction("Jump");
        _crouch = _input.actions.FindAction("Crouch");
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        var cameraInput = new CameraInput 
        { 
            look = _look.ReadValue<Vector2>() 
        };

        playerCamera.UpdateRotation(cameraInput);

        var characterInput = new CharacterInput
        {
            rotation = playerCamera.transform.rotation,
            move = _move.ReadValue<Vector2>(),
            jump = _jump.WasPerformedThisFrame(),
            jumpSustain = _jump.IsPressed(),
            crouch = _crouch.WasPerformedThisFrame() ? CrouchInput.Toggle : CrouchInput.None

        };

        playerCharacter.UpdateInput(characterInput);
        playerCharacter.UpdateBody(deltaTime);

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            var ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                Teleport(hitInfo.point);
            }
        }
#endif
    }

    private void LateUpdate()
    {
        var deltaTime = Time.deltaTime;
        var cameraTarget = playerCharacter.GetCameraTarget();
        var state = playerCharacter.GetState();
        playerCamera.UpdatePosition(playerCharacter.GetCameraTarget());
        cameraSpring.UpdateSpring(deltaTime, cameraTarget.up);
        cameraLean.UpdateLean(deltaTime, state.stance is Stance.Slide, state.acceleration, cameraTarget.up);
    }

    public void Teleport(Vector3 position)
    {
        playerCharacter.SetPosition(position);
    }
}
