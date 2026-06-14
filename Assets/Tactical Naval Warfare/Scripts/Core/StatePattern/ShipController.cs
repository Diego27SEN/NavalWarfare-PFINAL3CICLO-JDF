using UnityEngine;
using UnityEngine.EventSystems;

public class ShipController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float maxMoveDistance = 10f;

    [Header("Turn System")]
    public bool InTurn = false;

    [SerializeField] private PlayerInputSystem playerInputSystem;

    private Vector2 moveInput;
    private Vector3 moveDirection;


    private void OnEnable()
    {
        playerInputSystem.OnMove += SetMoveInput;
    }

    private void OnDisable()
    {
        playerInputSystem.OnMove -= SetMoveInput;
    }

    private void Update()
    {
        if (!InTurn) return;

        HandleMovement();
    }

    public void SetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void HandleMovement()
    {
        moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);
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