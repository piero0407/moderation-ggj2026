using UnityEngine;

public class WindowStateData : MonoBehaviour
{
    [SerializeField] private GameManager.GameState windowState;
    public GameManager.GameState WindowState { get => windowState;}
}
