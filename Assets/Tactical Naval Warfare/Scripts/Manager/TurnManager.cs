using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class TurnManager : MonoBehaviour
{
    [Title("Ships")]
    [SerializeField] private ShipController[] ships;

    [Title("Gameplay Camera")]
    [SerializeField] private CinemachineCamera gameplayCam;
    [SerializeField] private float turnDuration = 20f;
    [SerializeField] private float currentTime;
    [SerializeField] private int currentTurn;

    private TurnNode currentNode;
    private InputSystem_Actions inputs;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputs.Enable();

        inputs.Player.Next.performed += OnNextTurn;
    }
    private void OnDisable()
    {
        inputs.Player.Next.performed -= OnNextTurn;

        inputs.Disable();
    }

    private void Start()
    {
        CreateCircularList();

        StartTurn();
    }

    private void OnNextTurn(InputAction.CallbackContext context)
    {
        NextTurn();
    }

    private void CreateCircularList()
    {
        TurnNode firstNode = null;

        TurnNode previousNode = null;

        for (int i = 0; i < ships.Length; i++)
        {
            TurnNode newNode = new TurnNode(ships[i]);

            if (firstNode == null)
            {
                firstNode = newNode;
            }

            if (previousNode != null)
            {
                previousNode.next = newNode;

                newNode.previous = previousNode;
            }

            previousNode = newNode;
        }

        previousNode.next = firstNode;

        firstNode.previous = previousNode;

        currentNode = firstNode;
    }

    private void StartTurn()
    {
        currentNode.ship.StartTurn();

        ChangeCameraTarget();
    }

    [Button]
    public void NextTurn()
    {
        currentNode.ship.EndTurn();

        currentNode = currentNode.next;

        currentNode.ship.StartTurn();

        currentTurn++;

        ChangeCameraTarget();
    }

    private void ChangeCameraTarget()
    {
        gameplayCam.Follow =currentNode.ship.transform;

        gameplayCam.LookAt = currentNode.ship.transform;
    }
}