using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Scriptable_Objects_Architecture.Runtime.Variables;

public class PopUpController : MonoBehaviour
{
    [SerializeField] DialogsData dialogsData;
    [SerializeField, TextArea] private string _popUpText;
    string popUpText;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private float wordFrequency = 30f;
    [SerializeField] private float loopBubbleDuration = 5f;
    [SerializeField] private float introBubbleDuration = 3f;
    [SerializeField] private FloatVariable sanity;
    private float _timer = 0f;

    private bool doingAnimation = false;
    private int introDialog = 0;

    int currentLetterIndex = 0;

    void OnValidate()
    {
        bubbleText.text = popUpText;
    }

    void Start()
    {
        _timer = introBubbleDuration;
    }
    void Update()
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.GameState.Start:
            case GameManager.GameState.Evidence:
            case GameManager.GameState.Notepad:
            case GameManager.GameState.LivestreamMax:
            case GameManager.GameState.None:
            case GameManager.GameState.Livestream:
            if(doingAnimation)
                TextAnimation();
            else
                GetNewTextIfNeeded();
                break;
            default:
                break;
        }
    }

    public void SetPopUpText(string dialogtext)
    {
        _popUpText = dialogtext;
        bubbleText.text = "";
        currentLetterIndex = 0;
        _timer = 0f;
        doingAnimation = true;
    }

    private void GetNewTextIfNeeded()
    {
        _timer += Time.deltaTime;
        if(introDialog < dialogsData.StartingDialogs.Length)
        {
            if (_timer >= introBubbleDuration)
            {
                SetPopUpText(dialogsData.StartingDialogs[introDialog]);
                introDialog++;
                if(introDialog > dialogsData.StartingDialogs.Length - 1)
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
                int tempExpression = (int)(Mathf.Clamp01(sanity.Value * -1 + 1) * 6);

                switch (tempExpression)
                {
                    case 0:
                        SetPopUpText(dialogsData.GoodDialogs[Random.Range(0, dialogsData.GoodDialogs.Length)]);
                        break;
                    case 1:
                    case 2:
                    case 3:
                        SetPopUpText(dialogsData.NeutralDialogs[Random.Range(0, dialogsData.NeutralDialogs.Length)]);
                        break;
                    case 4:
                    case 5:
                        SetPopUpText(dialogsData.BadDialogs[Random.Range(0, dialogsData.BadDialogs.Length)]);
                        break;
                    case 6:
                        break;
                    default:
                        break;
                }
                _timer = 0f;
            }
        }
    }

    private void TextAnimation()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1 / wordFrequency && currentLetterIndex < _popUpText.Length)
        {
            currentLetterIndex++;
            bubbleText.text = _popUpText.Substring(0, currentLetterIndex);
            if (currentLetterIndex >= _popUpText.Length)
            {
                currentLetterIndex = _popUpText.Length - 1;
                doingAnimation = false;
            }
            _timer = 0f;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(bubbleText.rectTransform);
    }
}
