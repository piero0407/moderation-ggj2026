using UnityEngine;

public class ToggleWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] availableWindows;
    [SerializeField] private GameManager.GameState[] stateWindows;
    
    Transform parentTransform;

    void Start()
    {
        parentTransform = availableWindows[0].transform.parent;
    }

    public void ToggleWindowVisibility(int id)
    {
        if (!availableWindows[id].activeSelf)
        {
            availableWindows[id].SetActive(true);
            availableWindows[id].transform.SetAsLastSibling();
        }
        else
        {
            if (availableWindows[id].transform.GetSiblingIndex() == availableWindows.Length - 1)
            {
                availableWindows[id].transform.SetAsFirstSibling();
                availableWindows[id].SetActive(false);
            }
            else
            {
                availableWindows[id].transform.SetAsLastSibling();
            }
        }

        for (int i = 0; i < availableWindows.Length; i++)
        {
            if (availableWindows[i].transform == parentTransform.GetChild(availableWindows.Length - 1))
            {
                if (availableWindows[i].activeSelf)
                {
                    GameManager.Instance.ChangeState(stateWindows[i]);
                }
                else
                {
                    GameManager.Instance.ChangeState(GameManager.GameState.None);
                }
                break;
            }
        }
    }
}
