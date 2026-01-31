using UnityEngine;
using TMPro;

public class PopUpController : MonoBehaviour
{
    [SerializeField, TextArea] public string popUpText;
    [SerializeField] private TextMeshProUGUI bubbleText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnValidate()
    {
        bubbleText.text = popUpText;
    }

    // Update is called once per frame
    void Update()
    {
        bubbleText.text = popUpText;
    }
}
