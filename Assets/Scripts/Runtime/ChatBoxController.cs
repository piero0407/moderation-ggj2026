using System.Dynamic;
using UnityEngine;

public class ChatBoxController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject cannotDoTask;
    [SerializeField] private ChatData chatTextData;
    [SerializeField] private ChatData badChatTextData;
    [SerializeField] private ChatData spamChatTextData;
    [SerializeField] private ChatData evidenceChatTextData;
    [Header("Prefabs")]
    [SerializeField] private GameObject chatTextPrefab;
    [SerializeField] private GameObject badChatTextPrefab;
    [SerializeField] private GameObject spamChatTextPrefab;
    [SerializeField] private GameObject evidenceChatTextPrefab;


    [Header("Variables")]
    [SerializeField] private Vector2 randomTimer = new Vector2(.5f, 3f);
    [SerializeField, Range(0f, 1f)] private float badChatpercent = 0.4f;
    [SerializeField, Range(0f, 1f)] private float spamChatpercent = 0.25f;
    [SerializeField, Range(0f, 1f)] private float evidenceChatpercent = 0.9f;

    [SerializeField] int maxChatMessages = 20;
    private float _timer;
    private float _currentTime;

    void Start()
    {
        _timer = Random.Range(randomTimer.x, randomTimer.y);
    }

    void Update()
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.GameState.Evidence:
            case GameManager.GameState.Notepad:
            case GameManager.GameState.LivestreamMax:
            case GameManager.GameState.None:
                if (!cannotDoTask.activeSelf) cannotDoTask.SetActive(true);
                SpawnChatText();
                break;
            case GameManager.GameState.Livestream:
                if (cannotDoTask.activeSelf && GameManager.Instance.moderatorPerms) cannotDoTask.SetActive(false);
                if (!GameManager.Instance.moderatorPerms) cannotDoTask.SetActive(true);
                SpawnChatText();
                break;
            default:
                break;
        }
    }

    private void SpawnChatText()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _timer)
        {
            _currentTime = 0;
            _timer = Random.Range(randomTimer.x, randomTimer.y);

            if (!GameManager.Instance.eventTime)
            {
                badChatpercent = -1.0f;
                spamChatpercent = -1.0f;
            }
            else
            {
                badChatpercent = 0.4f;
                spamChatpercent = 0.25f;
            }

            float range = Random.Range(0f, 1f);

            if (range <= badChatpercent || range <= spamChatpercent)
            {
                ChatTextController chatTextController = Instantiate(range >= spamChatpercent ? badChatTextPrefab : spamChatTextPrefab, content.transform).GetComponent<ChatTextController>();
                if (range >= spamChatpercent)
                {
                    chatTextController.SetChatText(
                        badChatTextData.Users[Random.Range(0, badChatTextData.Users.Length)],
                        badChatTextData.ChatLines[Random.Range(0, badChatTextData.ChatLines.Length)]
                    );
                } else
                {
                    chatTextController.SetChatText(
                        spamChatTextData.Users[Random.Range(0, spamChatTextData.Users.Length)],
                        spamChatTextData.ChatLines[Random.Range(0, spamChatTextData.ChatLines.Length)]
                    );
                }
            }
            else
            {
                if (range < evidenceChatpercent)
                {
                    ChatTextController chatTextController = Instantiate(chatTextPrefab, content.transform).GetComponent<ChatTextController>();
                    chatTextController.SetChatText(
                        chatTextData.Users[Random.Range(0, chatTextData.Users.Length)],
                        chatTextData.ChatLines[Random.Range(0, chatTextData.ChatLines.Length)]
                    );
                } else
                {
                    ChatTextController chatTextController = Instantiate(evidenceChatTextPrefab, content.transform).GetComponent<ChatTextController>();
                    chatTextController.SetChatText(
                        evidenceChatTextData.Users[Random.Range(0, evidenceChatTextData.Users.Length)],
                        evidenceChatTextData.ChatLines[Random.Range(0, evidenceChatTextData.ChatLines.Length)]
                    );
                }
            }

            if (content.transform.childCount > maxChatMessages)
            {
                Destroy(content.transform.GetChild(0).gameObject);
            }
        }
    }
}
