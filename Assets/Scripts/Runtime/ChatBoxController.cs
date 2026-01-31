using UnityEngine;

public class ChatBoxController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject content;
    [SerializeField] private ChatData chatTextData;
    [Header("Prefabs")]
    [SerializeField] private GameObject chatTextPrefab;

    [Header("Variables")]
    [SerializeField] private Vector2 randomTimer = new Vector2(.5f, 3f);
    private float _timer;
    private float _currentTime;

    void Start()
    {
        _timer = Random.Range(randomTimer.x, randomTimer.y);
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _timer)
        {
            SpawnChatText();
            _currentTime = 0;
            _timer = Random.Range(randomTimer.x, randomTimer.y);
        }
    }

    private void SpawnChatText()
    {
        ChatTextController chatTextController = Instantiate(chatTextPrefab, content.transform).GetComponent<ChatTextController>();
        chatTextController.SetChatText(
            chatTextData.Users[Random.Range(0, chatTextData.Users.Length)],
            chatTextData.ChatLines[Random.Range(0, chatTextData.ChatLines.Length)]
        );

        if (content.transform.childCount > 12)
        {
            RemoveChat();
        }
    }

    private void RemoveChat()
    {
        Destroy(content.transform.GetChild(0).gameObject);
    }
}
