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

    private CannonController cannonController;

    private Vector2 moveInput;
    private Vector3 moveDirection;

    private void Awake()
    {
        cannonController = GetComponentInChildren<CannonController>();
        playerInputSystem.enabled = false; // bloqueado hasta que sea su turno
    }

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

        if (moveDirection == Vector3.zero) return;

        // Movimiento relativo a la cámara
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        // Aplanar para ignorar la inclinación vertical de la cámara
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 worldDirection = (camForward * moveDirection.z + camRight * moveDirection.x);

        // Mover
        transform.position += worldDirection * moveSpeed * Time.deltaTime;

        // Rotar suave hacia donde se mueve
        Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
        transform.rotation = Quaternion.Slerp( transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
    }
    public void StartTurn()
    {
        InTurn = true;
        playerInputSystem.enabled = true;  // solo este barco escucha
        cannonController?.EnableCannon();
        Debug.Log(name + " START TURN");
    }

    public void EndTurn()
    {
        InTurn = false;
        playerInputSystem.enabled = false; // deja de escuchar
        cannonController?.DisableCannon();
        Debug.Log(name + " END TURN");
    }
}