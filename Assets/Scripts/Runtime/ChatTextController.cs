using Scriptable_Objects_Architecture.Runtime.Variables;
using TMPro;
using UnityEngine;

public class ChatTextController : MonoBehaviour
{
    public enum ChatBoxType
    {
        Normal,
        FWords
    }
    
    [SerializeField] private TextMeshProUGUI chatText;
    [SerializeField] public ChatBoxType chatBoxType;
    [SerializeField] private FloatVariable sanity;

    bool _banned = false;
    public bool Banned { get => _banned; private set => _banned = value; }

    public void SetChatText(string userText, string messageText)
    {
        chatText.text = $"<b>{userText}:</b> {messageText}";
    }

    public void OnClick()
    {
        if (Banned) return;
        Banned = true;

        if (chatBoxType == ChatBoxType.FWords)
        {
            Debug.Log("Banned bad chat message");
            sanity.Value += 0.1f;
        }
        else
        {
            Debug.Log("Banned normal chat message");
            sanity.Value -= 0.1f;
        }

        chatText.text = "<i>This message has been removed.</i>";
        chatText.color = Color.gray;
    }
}
