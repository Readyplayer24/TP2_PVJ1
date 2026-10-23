using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 movementInput;

    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.7f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    private Vector3 velocity;

    public float Speed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    [Header("Raycast de Suelo (Ground Check)")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private bool isGrounded;

    [Header("Raycast de Interacción")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionMask;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Keyboard.current != null)
        {
            float moveX = 0f;
            float moveZ = 0f;

            if (Keyboard.current.wKey.isPressed) moveZ += 1f;
            if (Keyboard.current.sKey.isPressed) moveZ -= 1f;
            if (Keyboard.current.aKey.isPressed) moveX -= 1f;
            if (Keyboard.current.dKey.isPressed) moveX += 1f;

            bool isSprinting = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;

            float currentSpeed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * currentSpeed * Time.deltaTime);

            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                CheckInteractions();
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void CheckInteractions()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactionMask))
        {
            Debug.Log("¡Objeto interactivo detectado: " + hit.collider.name + "!");
        }
    }
}