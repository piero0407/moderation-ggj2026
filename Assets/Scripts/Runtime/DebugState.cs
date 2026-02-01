using TMPro;
using UnityEngine;

public class DebugState : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateText;
    
    void Update()
    {
        stateText.text = "Current State: " + GameManager.Instance.CurrentState.ToString();
    }
}
