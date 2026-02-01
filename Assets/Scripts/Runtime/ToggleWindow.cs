using System;
using System.Threading.Tasks;
using UnityEngine;

public class ToggleWindow : MonoBehaviour
{
    [SerializeField] private WindowStateData[] windows;
    
    Transform parentTransform;

    void Start()
    {
        Debug.Log(windows,this);
        parentTransform = windows[0].transform.parent;
    }

    public void ToggleWindowVisibility(int id)
    {
        if (!windows[id].gameObject.activeSelf)
        {
            windows[id].gameObject.SetActive(true);
            windows[id].transform.SetAsLastSibling();
        }
        else
        {
            if (windows[id].transform.GetSiblingIndex() == windows.Length - 1)
            {
                windows[id].transform.SetAsFirstSibling();
                windows[id].gameObject.SetActive(false);
            }
            else
            {
                windows[id].transform.SetAsLastSibling();
            }
        }

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
}
