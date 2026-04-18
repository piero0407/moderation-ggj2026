using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Scriptable_Objects_Architecture.Runtime.Variables;
using System.Collections;

public class PopUpController : MonoBehaviour
{
    [Header("Data & Settings")]
    [SerializeField] private DialogsData dialogsData;
    [SerializeField] private FloatVariable sanity;
    [SerializeField] private float loopBubbleDuration = 5f;
    [SerializeField] private float introBubbleDuration = 3f;

    [Header("UI References")]
    [SerializeField] private GameObject bubblePrefab;
    [SerializeField] private Transform bubbleContainer;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private float scrollSpeed = 5f;
    [SerializeField] private int maxHistoryCount = 5;

    private float _timer = 0f;
    private int introDialog = 0;

    void Start()
    {
        _timer = introBubbleDuration;
    }

    void Update()
    {
        if (IsStateValidForDialog())
        {
            GetNewTextIfNeeded();
        }
    }

    private bool IsStateValidForDialog()
    {
        var state = GameManager.Instance.CurrentState;
        return state == GameManager.GameState.Start ||
               state == GameManager.GameState.Evidence ||
               state == GameManager.GameState.Notepad ||
               state == GameManager.GameState.LivestreamMax ||
               state == GameManager.GameState.None ||
               state == GameManager.GameState.Livestream;
    }

    public void CreateNewBubble(string dialogText)
    {
        GameObject newBubble = Instantiate(bubblePrefab, bubbleContainer);

        TextMeshProUGUI textComp = newBubble.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null)
        {
            textComp.text = dialogText;
        }

        if (bubbleContainer.childCount > maxHistoryCount)
        {
            Destroy(bubbleContainer.GetChild(0).gameObject);
        }

        // Start the smooth scroll to the bottom
        StopAllCoroutines();
        StartCoroutine(SmoothScrollToBottom());
    }
    
    private IEnumerator SmoothScrollToBottom()
    {
        // Wait for the end of the frame so the Layout Group can calculate the new size
        yield return new WaitForEndOfFrame();

        float targetValue = 0f; // 0 is bottom in a vertical ScrollRect
        
        while (Mathf.Abs(scrollRect.verticalNormalizedPosition - targetValue) > 0.01f)
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition, 
                targetValue, 
                Time.deltaTime * scrollSpeed
            );
            yield return null;
        }
        
        scrollRect.verticalNormalizedPosition = targetValue;
    }

    private void GetNewTextIfNeeded()
    {
        _timer += Time.deltaTime;

        if (introDialog < dialogsData.StartingDialogs.Length)
        {
            if (_timer >= introBubbleDuration)
            {
                CreateNewBubble(dialogsData.StartingDialogs[introDialog]);
                introDialog++;

                if (introDialog >= dialogsData.StartingDialogs.Length)
                {
                    GameManager.Instance.ChangeState(GameManager.GameState.Livestream);
                }
                _timer = 0f;
            }
        }
        else
        {
            if (_timer >= loopBubbleDuration)
            {
                string selectedText = GetRandomDialogBasedOnSanity();
                if (!string.IsNullOrEmpty(selectedText))
                {
                    CreateNewBubble(selectedText);
                }
                _timer = 0f;
            }
        }
    }

    private string GetRandomDialogBasedOnSanity()
    {
        int tempExpression = (int)(Mathf.Clamp01(sanity.Value * -1 + 1) * 6);

        switch (tempExpression)
        {
            case 0:
                return dialogsData.GoodDialogs[Random.Range(0, dialogsData.GoodDialogs.Length)];
            case 1:
            case 2:
            case 3:
                return dialogsData.NeutralDialogs[Random.Range(0, dialogsData.NeutralDialogs.Length)];
            case 4:
            case 5:
                return dialogsData.BadDialogs[Random.Range(0, dialogsData.BadDialogs.Length)];
            default:
                return null;
        }
    }
}
