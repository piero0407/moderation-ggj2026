using UnityEngine;

[CreateAssetMenu(fileName = "DialogsData", menuName = "Scriptable Objects/DialogsData")]
public class DialogsData : ScriptableObject
{
    [SerializeField] private string[] startingDialogs;
    public string[] StartingDialogs { get => startingDialogs; }
    [SerializeField] private string[] loopingDialogs;
    public string[] LoopingDialogs { get => loopingDialogs; }
}
