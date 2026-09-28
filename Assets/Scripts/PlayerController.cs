using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.15f;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        // WASD
        moveInput = Vector2.zero;
        if (keyboard.wKey.isPressed) moveInput.y = 1;
        if (keyboard.sKey.isPressed) moveInput.y = -1;
        if (keyboard.aKey.isPressed) moveInput.x = -1;
        if (keyboard.dKey.isPressed) moveInput.x = 1;

        // Normalize so diagonal isn't faster
        if (moveInput.magnitude > 1f)
            moveInput.Normalize();

        // Jump
        if (keyboard.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}