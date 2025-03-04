using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] Camera cam;

    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float gravity = -9.8f;
    [SerializeField] float rotationSpeed = 400f;
    [SerializeField] float mouseSensitivityX = 0.5f;
    [SerializeField] float maxRotationX = 85f;
    [SerializeField] int health = 10;

    [SerializeField] KeyCode jumpButton = KeyCode.Space;

    private float x = 0;

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

        y += horizontalInput * rotationSpeed * mouseSensitivityX * Time.deltaTime;
        x += -verticalInput * rotationSpeed * mouseSensitivityX * Time.deltaTime;

        x = Mathf.Clamp(x, -maxRotationX, maxRotationX);

        transform.eulerAngles = new Vector3(0, y, 0);
        cam.transform.localRotation = Quaternion.Euler(x, 0, 0);
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
        Debug.Log("Player got hit for " + damage + " damage");
        health -= damage;
    }
}
