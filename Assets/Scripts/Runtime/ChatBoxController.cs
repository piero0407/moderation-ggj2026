using UnityEngine;

public class ChatBoxController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject cannotDoTask;
    [SerializeField] private ChatData chatTextData;
    [Header("Prefabs")]
    [SerializeField] private GameObject chatTextPrefab;
    

    [Header("Variables")]
    [SerializeField] private Vector2 randomTimer = new Vector2(.5f, 3f);
    private float _timer;
    private float _currentTime;
    private bool cannotTask;

    void Start()
    {
        _timer = Random.Range(randomTimer.x, randomTimer.y);
        cannotTask = false;
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

        if (!cannotTask && cannotDoTask.activeSelf) cannotDoTask.SetActive(false);
        else if (cannotTask && !cannotDoTask.activeSelf) cannotDoTask.SetActive(true);
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
        if (!cannotTask) return;
        Destroy(content.transform.GetChild(0).gameObject);
    }

    public void DisableTaskAction(bool revert = false)
    {
        if (revert)
        {
            cannotTask = false;
            cannotDoTask.SetActive(false);
        } else
        {
            cannotTask = true;
            cannotDoTask.SetActive(true);
        }
    }
}
