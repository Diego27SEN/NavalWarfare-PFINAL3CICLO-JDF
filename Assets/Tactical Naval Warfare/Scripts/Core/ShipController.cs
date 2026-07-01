using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipController : MonoBehaviour
{
    [SerializeField] private ShipType _shipType;
    public ShipType shipType => _shipType;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 5f;
    public float maxMoveDistance = 10f;

    [Header("Turn System")]
    public bool InTurn = false;

    [Header("Targeting System")]
    public List<Transform> enemies; 

    [SerializeField] private PlayerInputSystem playerInputSystem;

    private CannonController cannonController;
    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        cannonController = GetComponentInChildren<CannonController>();
        playerInputSystem.enabled = false;
    }


    private void OnEnable()
    {
        playerInputSystem.OnMove += SetMoveInput;
    }

    private void OnDisable()
    {
        playerInputSystem.OnMove -= SetMoveInput;
    }

    private void FixedUpdate()
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
        if (moveInput.y < 0) moveInput.y = 0;

        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        if (moveDirection == Vector3.zero)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 worldDirection = (camForward * moveDirection.z + camRight * moveDirection.x).normalized;

        
        rb.linearVelocity = worldDirection * moveSpeed;

        // Rotar suave
        Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }
    public void StartTurn()
    {
        InTurn = true;
        rb.isKinematic = false; // dinámico mientras es su turno
        rb.useGravity = false;
        playerInputSystem.enabled = true;
        cannonController?.EnableCannon();
    }

    public void EndTurn()
    {
        InTurn = false;
        rb.isKinematic = true; // vuelve a kinematic al terminar
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        playerInputSystem.enabled = false;
        cannonController?.DisableCannon();
    }

    public void AttemptAttack()
    {
        // Validaciones
        if (!InTurn) return;
        if (enemies == null || enemies.Count == 0)
        {
            Debug.Log("No hay enemigos para atacar.");
            return;
        }

        // Ordenamos enemigos
        OrderEnemiesByDistance();

        // Atacamos al primero (el más cercano)
        Transform target = enemies[0];

        if (cannonController != null)
        {
            cannonController.Fire(target);
        }
    }

    private void OrderEnemiesByDistance()
    {
        int n = enemies.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                float dist1 = Vector3.Distance(transform.position, enemies[j].position);
                float dist2 = Vector3.Distance(transform.position, enemies[j + 1].position);

                if (dist1 > dist2) // Intercambio
                {
                    Transform temp = enemies[j];
                    enemies[j] = enemies[j + 1];
                    enemies[j + 1] = temp;
                }
            }
        }
    }
}