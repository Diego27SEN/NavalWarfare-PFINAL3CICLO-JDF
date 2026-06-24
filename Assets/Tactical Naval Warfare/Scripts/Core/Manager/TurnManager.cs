using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [Title("Ships")]
    [SerializeField] private ShipController[] ships;

    [Title("Gameplay Camera")]
    [SerializeField] private CinemachineCamera gameplayCam;
    public float turnDuration = 20f;
    public float currentTime;
    [SerializeField] private int currentTurn;
    [SerializeField] private CinemachineCamera BulletCam;

    [SerializeField] private TextMeshProUGUI[] turnTexts = new TextMeshProUGUI[4];

    [SerializeField] private TextMeshProUGUI activeShipHealthText;

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
            currentTime = float.MaxValue;
            GameManager.Instance.ForceEndTurn(); // nuevo método sin verificar dado
        }

        UpdateActiveShipHealthUI();
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

            if (string.IsNullOrEmpty(newNode.ship.gameObject.name) || newNode.ship.gameObject.name.StartsWith("GameObject"))
            {
                newNode.ship.gameObject.name = "Barco " + (i + 1);
            }

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

        UpdateTurnUIBoxes();
        UpdateActiveShipHealthUI();
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

    private void UpdateTurnUIBoxes()
    {
        TurnNode tempNode = currentNode;

        for (int i = 0; i < turnTexts.Length; i++)
        {
            if (turnTexts[i] != null && tempNode != null)
            {
                turnTexts[i].text = tempNode.ship.gameObject.name;
                tempNode = tempNode.next;
            }
        }
    }

    public void UpdateActiveShipHealthUI()
    {
        if (activeShipHealthText == null || currentNode == null || currentNode.ship == null)
        {
            return;
        }

        ShipHealth healthComponent = currentNode.ship.GetComponentInChildren<ShipHealth>();

        if (healthComponent != null)
        {
            float current = healthComponent.GetCurrentHealth();
            float max = healthComponent.shipData != null ? healthComponent.shipData.hpMaximum : 400f;

            if (current == 0 && max > 0)
            {
                current = max;
            }

            activeShipHealthText.text = $"{current} / {max}";
        }
        else
        {
            Debug.LogWarning($"[TurnManager] Ojo: No se encontró ShipHealth en {currentNode.ship.gameObject.name}");
            activeShipHealthText.text = "--- / ---";
        }
    }
}