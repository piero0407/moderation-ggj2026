using TMPro;
using UnityEngine;

public class ChatTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI user;
    [SerializeField] private TextMeshProUGUI message;
    bool _banned = false;

    private string chatText;

    public void SetChatText(string userText, string messageText)
    {
        user.text = $"<b>{userText}: </b>";
        message.text = messageText;
        chatText = userText + messageText;
    }

    public void OnClick()
    {
        if (_banned) return;
        
        _banned = true;
        user.text = "<i>This user has been banned.</i>";
        message.text = "";
        Destroy(message.gameObject);
        //LayoutRebuilder.ForceRebuildLayoutImmediate(message.rectTransform);
    }
}
