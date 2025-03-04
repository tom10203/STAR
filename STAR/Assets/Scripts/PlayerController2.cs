using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] Camera cam;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] float mouseSensitivityX = 10f;
    [SerializeField] float maxRotationX = 30f;
    [SerializeField] int health = 10;

    [SerializeField] KeyCode jumpButton = KeyCode.Space;

    CharacterController characterController;
    Vector3 direction;


    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        Move();
        Rotate();
    }

    void Rotate()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        float y = transform.eulerAngles.y;
        float x = cam.transform.eulerAngles.x;

        y += horizontalInput * rotationSpeed * Time.deltaTime;
        x -= verticalInput * rotationSpeed * Time.deltaTime;

        x = Mathf.Clamp(x, -maxRotationX, maxRotationX);

        transform.eulerAngles = new Vector3(0, y, 0);
        cam.transform.localEulerAngles = new Vector3(x, 0, 0);
        
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float y = direction.y;

        direction = (transform.forward * verticalInput + transform.right * horizontalInput) * moveSpeed;

        if (characterController.isGrounded)
        {
            if (Input.GetKeyDown(jumpButton))
            {
                y = jumpForce;
            }
            else
            {
                y = -0.005f;
            }
        }
        else
        {
            y += gravity * Time.deltaTime;
        }

        direction.y = y;

        characterController.Move(direction * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
