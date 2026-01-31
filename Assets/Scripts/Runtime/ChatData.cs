using UnityEngine;

[CreateAssetMenu(fileName = "ChatData", menuName = "Scriptable Objects/ChatData")]
public class ChatData : ScriptableObject
{
    [SerializeField] private string[] users;
    [SerializeField] private string[] chatLines;

    public string[] Users { get => users;}
    public string[] ChatLines { get => chatLines; }
}
