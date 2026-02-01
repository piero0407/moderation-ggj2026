using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    [SerializeField] private FloatVariable sanity;
    [SerializeField] private float naturalSanityDecrese = 0.1f;

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
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    void Update()
    {
        switch(CurrentState)
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
}
