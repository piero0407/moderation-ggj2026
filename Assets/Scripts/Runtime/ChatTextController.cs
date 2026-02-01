using System;
using Scriptable_Objects_Architecture.Runtime.Variables;
using TMPro;
using UnityEngine;

public class ChatTextController : MonoBehaviour
{
    public enum ChatBoxType
    {
        Normal,
        FWords,
        Spam,
        Special
    }
    
    [SerializeField] private TextMeshProUGUI chatText;
    [SerializeField] public ChatBoxType chatBoxType;
    [SerializeField] private FloatVariable sanity;

    bool _banned = false;
    public bool Banned { get => _banned; private set => _banned = value; }

    private String username;

    public void SetChatText(string userText, string messageText)
    {
        username = userText;
        chatText.text = $"<b>{userText}:</b> {messageText}";
    }

    public void OnClick()
    {
        if (Banned || GameManager.Instance.moderatorPerms == false) return;
        Banned = true;

        if (chatBoxType == ChatBoxType.FWords || chatBoxType == ChatBoxType.Spam)
        {
            sanity.Value += chatBoxType == ChatBoxType.FWords ? 0.08f : 0.04f;
            sanity.Value = Mathf.Clamp01(sanity.Value);
        }
        else if (chatBoxType == ChatBoxType.Normal)
        {
            sanity.Value -= 0.08f;
            sanity.Value = Mathf.Clamp01(sanity.Value);
        }
        else if (chatBoxType == ChatBoxType.Special)
        {
            GameManager.Instance.CallEvidenceCollect(1, username);
        }

        chatText.text = chatBoxType != ChatBoxType.Normal && chatBoxType != ChatBoxType.Special ? "<i>This message has been removed.</i>" : (chatBoxType != ChatBoxType.Normal ? "<i>Evidence obtained!</i>" : "<i>This message has been wrongfully removed.</i>");
        chatText.color = chatBoxType != ChatBoxType.Normal && chatBoxType != ChatBoxType.Special ? Color.gray : (chatBoxType != ChatBoxType.Normal ? Color.green : Color.darkRed);

        if (chatBoxType != ChatBoxType.Normal && GameManager.Instance.eventTime)
        {
            GameManager.Instance.taskCompletion += 0.05f;
            if (GameManager.Instance.taskCompletion >= 1.0f) GameManager.Instance.IncreaseComplexity();
        }
    }
}
