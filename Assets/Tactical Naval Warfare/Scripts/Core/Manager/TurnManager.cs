using Sirenix.OdinInspector;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

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

    private TurnNode currentNode;
    private InputSystem_Actions inputs;
    [SerializeField] private GameplayUI gameplayUI;

    public ShipController GetBarcoActual()
    {
        return currentNode?.ship;
    }

    private void Awake()
    {
        Instance = this;
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
        TurnNode firstNode = null;
        TurnNode previousNode = null;

        for (int i = 0; i < ships.Length; i++)
        {
            if (ships[i] == null) continue;

            TurnNode newNode = new TurnNode(ships[i]);
            newNode.ship.gameObject.name = GetShipName(ships[i], i);

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

        if (previousNode != null && firstNode != null)
        {
            previousNode.next = firstNode;
            firstNode.previous = previousNode;
            currentNode = firstNode;
        }
    }

    public void RemoveShipFromTurnOrder(ShipController deadShip)
    {
        if (currentNode == null) return;

        TurnNode tempNode = currentNode;
        bool found = false;

        for (int i = 0; i < ships.Length; i++)
        {
            if (tempNode.ship == deadShip)
            {
                found = true;
                break;
            }
            tempNode = tempNode.next;
        }

        if (found)
        {
            Debug.Log($"[TurnManager] Eliminando a {deadShip.gameObject.name} del orden de turnos.");

            if (tempNode.next == tempNode)
            {
                currentNode = null;
                return;
            }

            tempNode.previous.next = tempNode.next;
            tempNode.next.previous = tempNode.previous;

            if (currentNode == tempNode)
            {
                currentNode = tempNode.previous;
            }

            UpdateTurnUIBoxes();
        }
    }

    private void StartTurn()
    {
        if (currentNode == null || currentNode.ship == null) return;

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
        if (currentNode != null && currentNode.ship != null)
        {
            currentNode.ship.EndTurn();
        }

        if (CheckWinner()) return;

        if (currentNode != null)
        {
            currentNode = currentNode.next;
        }

        currentTurn++;
        StartTurn();
    }

    private void ChangeCameraTarget()
    {
        if (currentNode == null || currentNode.ship == null) return;
        gameplayCam.Follow = currentNode.ship.transform;
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
            if (turnTexts[i] != null)
            {
                if (tempNode != null && tempNode.ship != null && tempNode.ship.gameObject.activeSelf)
                {
                    turnTexts[i].text = tempNode.ship.gameObject.name;
                    Debug.Log($"Vuelta {i} -> Objeto: {tempNode.ship.name} | Su tipo asignado es: {tempNode.ship.shipType}");
                    tempNode = tempNode.next;
                }
                else
                {
                    turnTexts[i].text = "---";
                }
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

    private bool CheckWinner()
    {
        int alive = 0;
        ShipController winner = null;

        foreach (ShipController ship in ships)
        {
            if (ship == null) continue;

            ShipHealth health = ship.GetComponentInChildren<ShipHealth>();

            if (health != null && health.GetCurrentHealth() > 0 && ship.gameObject.activeSelf)
            {
                alive++;
                winner = ship;
            }
        }

        if (alive == 1 && winner != null)
        {
            playerNamesSO.SetWinner(winner.shipType);
            SceneManager.LoadScene("WinnerScene");
            return true;
        }
        return false;
    }
}