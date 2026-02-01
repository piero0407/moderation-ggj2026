using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }

    public Transform WindowArea  { get; set; }

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
}
