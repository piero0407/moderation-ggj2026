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
            sanity.Value += 0.08f;
        }
        else
        {
            Debug.Log("Banned normal chat message");
            sanity.Value -= 0.08f;
        }

        chatText.text = chatBoxType != ChatBoxType.Normal ? "<i>This message has been removed.</i>" : "<i>This message has been wrongfully removed.</i>";
        chatText.color = chatBoxType != ChatBoxType.Normal ? Color.gray : Color.darkRed;

        if (chatBoxType != ChatBoxType.Normal && GameManager.Instance.eventTime)
        {
            GameManager.Instance.taskCompletion += 0.05f;
            if (GameManager.Instance.taskCompletion >= 1.0f) GameManager.Instance.IncreaseComplexity();
        }
    }
}
