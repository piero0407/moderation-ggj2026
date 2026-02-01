using UnityEngine;

[CreateAssetMenu(fileName = "DialogsData", menuName = "Scriptable Objects/DialogsData")]
public class DialogsData : ScriptableObject
{
    [SerializeField] private string[] startingDialogs;
    public string[] StartingDialogs { get => startingDialogs; }
    [SerializeField] private string[] goodDialogs;
    public string[] GoodDialogs { get => goodDialogs; }
    [SerializeField] private string[] neutralDialogs;
    public string[] NeutralDialogs { get => neutralDialogs; }
    
    [SerializeField] private string[] badDialogs;
    public string[] BadDialogs { get => badDialogs; }
}
