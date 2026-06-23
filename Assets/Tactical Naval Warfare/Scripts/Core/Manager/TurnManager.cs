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
    public float currentTime;
    [SerializeField] private int currentTurn;
    [SerializeField] private CinemachineCamera BulletCam;

    private bool gameStarted = false;
    private bool timerPaused = false;

    private TurnNode currentNode;
    private InputSystem_Actions inputs;
    [SerializeField] private GameplayUI gameplayUI;

    private void Awake()
    {
        inputs = new InputSystem_Actions();
        BulletCameraController.bulletCam = BulletCam;
    }

    private void Update()
    {
        if (!gameStarted || timerPaused) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = float.MaxValue; // Evita que se llame repetidamente a EndTurn mientras se procesa el cambio de turno
            GameManager.Instance.EndTurn(); // Llama al método EndTurn del GameManager para manejar la lógica de fin de turno
        }
    }
    private void OnEnable()
    {
        inputs.Enable();

        inputs.Player.Next.performed += OnNextTurn;
    }
    private void OnDisable()
    {
        if (inputs == null) return;
        inputs.Player.Next.performed -= OnNextTurn;

        inputs.Disable();
    }

    private void Start()
    {
        CreateCircularList();

    }

    private void OnNextTurn(InputAction.CallbackContext context)
    {
        GameManager.Instance.EndTurn(); // Llama al método EndTurn del GameManager para manejar la lógica de fin de turno
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

        currentTime = turnDuration;

        ChangeCameraTarget();

        gameplayUI.UpdateActiveShip(currentNode.ship); // actualiza el botón

    }

    public void StartGame()
    {
        gameStarted = true;
        StartTurn();
    }
    [Button]
    public void NextTurn()
    {
        currentNode.ship.EndTurn();
        currentNode = currentNode.next;
        currentTurn++;
        StartTurn(); 
    }

    private void ChangeCameraTarget()
    {
        gameplayCam.Follow =currentNode.ship.transform;

        gameplayCam.LookAt = currentNode.ship.transform;
    }

    public void PauseTimer() 
    {
        timerPaused = true; 
    }
    public void ResumeTimer() 
    { 
        timerPaused = false; 
    }
}