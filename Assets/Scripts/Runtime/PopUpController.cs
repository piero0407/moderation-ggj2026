using UnityEngine;
using TMPro;
using System;

public class PopUpController : MonoBehaviour
{
    [SerializeField] DialogsData dialogsData;
    [SerializeField, TextArea] public string popUpText;
    string _popUpText;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private float textFrequency = 30f;
    [SerializeField] private float textTimer = 0f;

    private bool haveDialogReady = false;

    int currentLetterIndex = 0;

    void OnValidate()
    {
        bubbleText.text = popUpText;
    }

    void Start()
    {
        SetPopUpText(true, 0);
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
        if(haveDialogReady)
            TextAnimation();
        else
            GetNewTextIfNeeded();
    }

    private void GetNewTextIfNeeded()
    {
    }

    private void TextAnimation()
    {
        textTimer += Time.deltaTime;
        if (textTimer >= 1 / textFrequency && currentLetterIndex < _popUpText.Length)
        {
            currentLetterIndex++;
            bubbleText.text = _popUpText.Substring(0, currentLetterIndex);
            if (currentLetterIndex >= _popUpText.Length)
            {
                currentLetterIndex = _popUpText.Length - 1;
                haveDialogReady = false;
            }
            textTimer = 0f;
        }
    }
}
