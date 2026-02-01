using UnityEngine;

public class MinimizeWindow : MonoBehaviour
{
    [SerializeField] private WindowStateData windowToToggle;
    Transform parentTransform;

    void Start()
    {
        parentTransform = windowToToggle.transform.parent;
    }

    public void DoMinimizeWindow()
    {
        if (windowToToggle != null)
        {
            windowToToggle.transform.SetAsFirstSibling();
            windowToToggle.gameObject.SetActive(false);

            // Set GameManager state based on the topmost active window
            var topmostWindow = parentTransform.GetChild(parentTransform.childCount - 1);
            if (topmostWindow.gameObject.activeSelf)
            {
                GameManager.Instance.ChangeState(topmostWindow.GetComponent<WindowStateData>()?.WindowState ?? GameManager.GameState.None);
            }
            else
            {
                GameManager.Instance.ChangeState(GameManager.GameState.None);
            }
        }
        else
        {
            Debug.LogWarning("Window to minimize is not assigned.");
        }
    }
}
