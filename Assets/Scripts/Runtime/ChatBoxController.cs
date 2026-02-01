using UnityEngine;

public class ChatBoxController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject cannotDoTask;
    [SerializeField] private ChatData chatTextData;
    [SerializeField] private ChatData badChatTextData;
    [Header("Prefabs")]
    [SerializeField] private GameObject chatTextPrefab;
    [SerializeField] private GameObject badChatTextPrefab;
    

    [Header("Variables")]
    [SerializeField] private Vector2 randomTimer = new Vector2(.5f, 3f);
    [SerializeField, Range(0f, 1f)] private float badChatpercent = 0.2f;
    [SerializeField] int maxChatMessages = 12;
    private float _timer;
    private float _currentTime;

    void Start()
    {
        _timer = Random.Range(randomTimer.x, randomTimer.y);
    }

    void Update()
    {
        switch(GameManager.Instance.CurrentState)
        {
            case GameManager.GameState.Evidence:
            case GameManager.GameState.Notepad:
            
                if (!cannotDoTask.activeSelf) cannotDoTask.SetActive(true);
                SpawnChatText();
                break;

            case GameManager.GameState.Livestream:
            case GameManager.GameState.LivestreamMax:

                if (cannotDoTask.activeSelf) cannotDoTask.SetActive(false);
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
            // Reset timer to a random value
            _currentTime = 0;
            _timer = Random.Range(randomTimer.x, randomTimer.y);

            if(Random.Range(0f, 1f) <= badChatpercent)
            {
                // Instantiate chat text
                ChatTextController chatTextController = Instantiate(badChatTextPrefab, content.transform).GetComponent<ChatTextController>();
                chatTextController.SetChatText(
                    badChatTextData.Users[Random.Range(0, badChatTextData.Users.Length)],
                    badChatTextData.ChatLines[Random.Range(0, badChatTextData.ChatLines.Length)]
                );
            }
            else
            {
                // Instantiate chat text
                ChatTextController chatTextController = Instantiate(chatTextPrefab, content.transform).GetComponent<ChatTextController>();
                chatTextController.SetChatText(
                    chatTextData.Users[Random.Range(0, chatTextData.Users.Length)],
                    chatTextData.ChatLines[Random.Range(0, chatTextData.ChatLines.Length)]
                );
            }

            // Limit chat messages to 12
            if (content.transform.childCount > maxChatMessages)
            {
                Destroy(content.transform.GetChild(0).gameObject);
            }
        }
    }
}
