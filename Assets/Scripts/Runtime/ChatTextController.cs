using TMPro;
using UnityEngine;

public class ChatTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI chatText;
    bool _banned = false;

    public void SetChatText(string userText, string messageText)
    {
        chatText.text = $"<b>{userText}:</b> {messageText}";
    }

    public void OnClick()
    {
        if (_banned) return;
        _banned = true;

        chatText.text = "<i>This message has been removed.</i>";
        //LayoutRebuilder.ForceRebuildLayoutImmediate(message.rectTransform);
    }
}
