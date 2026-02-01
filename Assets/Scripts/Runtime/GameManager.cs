using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public int evidenceCollected { get; private set; }
    public int tasksCompleted { get; private set; }
    public int totalTasks { get; private set; }

    public float timeSinceBegin;
    public float timeMultiplier { get; private set; }
    public bool eventTime;

    public float eventCooldown = 10.0f;
    public float taskCompletion = 0.0f;

    [SerializeField] private FloatVariable sanity;
    [SerializeField] private float naturalSanityDecrese = 0.001f;

    private void Awake() 
{ 
    // If there is an instance, and it's not me, delete myself.
    
    if (Instance != null && Instance != this) 
    { 
        Destroy(this); 
    } 
    else 
    { 
        Instance = this; 
    } 
}
    public enum GameState
    {
        Start,
        Livestream,
        LivestreamMax,
        Notepad,
        Evidence,
        None,
        Paused,
        GameOver
    }

    void Start()
    {
        CurrentState = GameState.Start;
        timeMultiplier = 1.0f;
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    void Update()
    {
        switch (CurrentState)
        {
            case GameState.Evidence:
            case GameState.Notepad:
            case GameState.Livestream:
            case GameState.LivestreamMax:
            case GameState.None:
                sanity.Value -= Time.deltaTime * naturalSanityDecrese;
                break;

            default:
                break;
        }
    }
    
    public void IncreaseComplexity()
    {
        timeMultiplier += 0.15f;
        eventTime = false;
        eventCooldown = 10.0f;

        totalTasks += 1;
        if (taskCompletion > 0.85f) tasksCompleted += 1;
    }
}
