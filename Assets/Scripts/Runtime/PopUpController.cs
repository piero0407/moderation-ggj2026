using UnityEngine;
using TMPro;

public class PopUpController : MonoBehaviour
{
    [SerializeField] DialogsData dialogsData;
    [SerializeField, TextArea] public string popUpText;
    string _popUpText;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private float textFrequency = 30f;
    [SerializeField] private float textTimer = 3f;
    private float _timer = 0f;

    private bool haveDialogReady = false;
    private int introDialog = 0;

    int currentLetterIndex = 0;

    void OnValidate()
    {
        bubbleText.text = popUpText;
    }

    void Start()
    {
    }

    public void SetPopUpText(bool isStartingDialog, int dialogIndex)
    {
        if (isStartingDialog)
        {
            _popUpText = dialogsData.StartingDialogs[dialogIndex];
        }
        else
        {
            _popUpText = dialogsData.LoopingDialogs[dialogIndex];
        }
        bubbleText.text = "";
        currentLetterIndex = 0;
        textTimer = 0f;
        haveDialogReady = true;
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
            if(haveDialogReady)
                TextAnimation();
            else
                GetNewTextIfNeeded();
                break;
            default:
                break;
        }
    }

    private void GetNewTextIfNeeded()
    {
        _timer += Time.deltaTime;
        if (_timer < textTimer)
        {
            if(introDialog < dialogsData.StartingDialogs.Length)
            {
                SetPopUpText(true, introDialog);
                haveDialogReady = true;
                introDialog++;
                if(introDialog >= dialogsData.StartingDialogs.Length - 1)
                {
                    GameManager.Instance.ChangeState(GameManager.GameState.Livestream);
                }
            }
            else
            {
                SetPopUpText(false, Random.Range(0, dialogsData.LoopingDialogs.Length));
                haveDialogReady = true;
            }
            _timer= 0f;
        }
    }

    private void TextAnimation()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1 / textFrequency && currentLetterIndex < _popUpText.Length)
        {
            currentLetterIndex++;
            bubbleText.text = _popUpText.Substring(0, currentLetterIndex);
            if (currentLetterIndex >= _popUpText.Length)
            {
                currentLetterIndex = _popUpText.Length - 1;
                haveDialogReady = false;
            }
            _timer = 0f;
        }
    }
}
