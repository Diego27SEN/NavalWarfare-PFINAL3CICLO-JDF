using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float maxMoveDistance = 10f;

    [Header("Turn System")]
    public bool InTurn = false;

    private Vector3 moveDirection;
    private InputSystem_Actions inputs;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Update()
    {
        if (!InTurn) return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 moveInput = inputs.Player.Move.ReadValue<Vector2>();

        moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public void StartTurn()
    {
        InTurn = true;
        Debug.Log(name + " START TURN");
    }

    public void EndTurn()
    {
        InTurn = false;
        Debug.Log(name + " END TURN");
    }
}