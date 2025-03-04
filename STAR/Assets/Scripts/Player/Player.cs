using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerCharacter playerCharacter;
    [SerializeField] private PlayerCamera playerCamera;

    [SerializeField] private PlayerInput _input;
    InputAction _move;
    InputAction _look;
    InputAction _jump;
    InputAction _crouch;

    private void Start()
    {
        playerCharacter.Initialise();
        playerCamera.Initialise(playerCharacter.GetCameraTarget());


        _move = _input.actions.FindAction("Move");
        _look = _input.actions.FindAction("Look");
        _jump = _input.actions.FindAction("Jump");
        _crouch = _input.actions.FindAction("Crouch");
    }

    private void Update()
    {


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
            crouch = _crouch.WasPerformedThisFrame() ? CrouchInput.Toggle : CrouchInput.None

        };

        playerCharacter.UpdateInput(characterInput);
        playerCharacter.UpdateBody();
    }

    private void LateUpdate()
    {
        playerCamera.UpdatePosition(playerCharacter.GetCameraTarget());
    }
}
