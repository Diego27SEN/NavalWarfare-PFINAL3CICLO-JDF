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

    [SerializeField] private TextMeshProUGUI currentTurnTitleText;

    [Title("Turn Order")]
    [SerializeField] private TextMeshProUGUI[] turnTexts = new TextMeshProUGUI[4];

    [SerializeField] private TextMeshProUGUI activeShipHealthText;
    [SerializeField] private PlayerNames playerNamesSO;

    private bool gameStarted = false;
    private bool timerPaused = false;

    private TurnNode firstNode; // NUEVO: guardamos referencia fija
    private int roundCount = 0; // NUEVO

    private TurnNode currentNode;
    private InputSystem_Actions inputs;
    [SerializeField] private GameplayUI gameplayUI;

    public ShipController GetBarcoActual()
    {
        return currentNode?.ship;
    }
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
            GameManager.Instance.ForceEndTurn();
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
        StartGame();
    }

    private void OnNextTurn(InputAction.CallbackContext context)
    {
        GameManager.Instance.EndTurn();
    }

    private void CreateCircularList()
    {
        TurnNode newFirstNode = null;
        TurnNode previousNode = null;

        for (int i = 0; i < ships.Length; i++)
        {
            if (ships[i] == null) continue;

            TurnNode newNode = new TurnNode(ships[i]);
            newNode.ship.gameObject.name = GetShipName(ships[i], i);

            if (newFirstNode == null) newFirstNode = newNode;
            if (previousNode != null)
            {
                previousNode.next = newNode;
                newNode.previous = previousNode;
            }
            previousNode = newNode;
        }

        if (previousNode != null && newFirstNode != null)
        {
            previousNode.next = newFirstNode;
            newFirstNode.previous = previousNode;
            currentNode = newFirstNode;
            firstNode = newFirstNode; //guardamos referencia al primer nodo
        }
    }

    public void StartTurn()
    {
        currentNode.ship.StartTurn();
        currentTime = turnDuration;
        ChangeCameraTarget();
        gameplayUI.UpdateActiveShip(currentNode.ship);

        UpdateCurrentTurnTitle();
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

        // NUEVO: detectar si completamos una vuelta
        if (currentNode == firstNode)
        {
            roundCount++;
            Debug.Log($"[TurnManager] Ronda completada. Total: {roundCount}");

            if (roundCount % 2 == 0)
            {
                CardsEventManager.Instance.StartCardEvent(ships);
                return; // el evento de cartas se encarga de llamar a StartTurn() cuando termine
            }
        }

        StartTurn();
    }

    private void ChangeCameraTarget()
    {
        gameplayCam.Follow = currentNode.ship.transform;
        gameplayCam.LookAt = currentNode.ship.transform;
    }

    public void FocusCameraOn(ShipController ship)
    {
        gameplayCam.Follow = ship.transform;
        gameplayCam.LookAt = ship.transform;
    }

    public void PauseTimer()
    {
        timerPaused = true;
    }

    public void ResumeTimer()
    {
        timerPaused = false;
    }

    private void UpdateCurrentTurnTitle()
    {
        if (currentTurnTitleText != null && currentNode != null && currentNode.ship != null)
        {
            currentTurnTitleText.text = currentNode.ship.gameObject.name;
        }
    }

    private void UpdateTurnUIBoxes()
    {
        if (currentNode == null) return;

        TurnNode tempNode = currentNode;

        for (int i = 0; i < turnTexts.Length; i++)
        {
            if (turnTexts[i] != null && tempNode != null && tempNode.ship != null)
            {
                turnTexts[i].text = tempNode.ship.gameObject.name;
                Debug.Log($"Vuelta {i} -> Objeto: {tempNode.ship.name} | Su tipo asignado es: {tempNode.ship.shipType}");
                tempNode = tempNode.next;
            }
        }
    }

    private string GetShipName(ShipController ship, int fallbackIndex)
    {
        if (ship == null) return "No Ship";

        ShipType currentType = ship.shipType;
        if (playerNamesSO != null && playerNamesSO.ShipNames.TryGetValue(currentType, out string customName) && !string.IsNullOrWhiteSpace(customName))
        {
            return customName;
        }

        return "Ship " + (fallbackIndex + 1);
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
            Debug.LogWarning($"[TurnManager] Ojo: No se encontro ShipHealth en {currentNode.ship.gameObject.name}");
            activeShipHealthText.text = "--- / ---";
        }
    }
}