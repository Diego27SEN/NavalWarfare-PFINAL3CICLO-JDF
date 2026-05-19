using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;


    [BoxGroup("Managers")]
    [SerializeField] private TurnManager turnManager;

    [BoxGroup("Managers")]
    [SerializeField] private UIManager uiManager;

    [BoxGroup("Managers")]
    [SerializeField] private AudioManager audioManager;

    [BoxGroup("Managers")]
    [SerializeField] private DadoSystem diceSystem;

    [BoxGroup("Managers")]
    [SerializeField] private TurnManager Cannon;

    [BoxGroup("Managers")]
    [SerializeField] private UIManager Ships;

    [BoxGroup("Managers")]
    [SerializeField] private AudioManager CardData;

    [BoxGroup("Managers")]
    [SerializeField] private DadoSystem MapData;

    //Game State

    [BoxGroup("Game State")]
    [ReadOnly]
    public bool gameStarted;

    [BoxGroup("Game State")]
    [ReadOnly]
    public bool gameFinished;

    [BoxGroup("Game State")]
    [ReadOnly]
    public int currentTurn = 1;



    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartGame();
    }

    [Button]
    public void StartGame()
    {
        gameStarted = true;

        Debug.Log("GAME STARTED");

        
    }


}