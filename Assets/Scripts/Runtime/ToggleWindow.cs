using System.Linq;
using UnityEngine;

public class ToggleWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] availableWindows;
    private int lastWhich = 1;

    public void ToggleWindowVisibility(int which)
    {
        if (which != 0)
        {
            GameObject obj = availableWindows[which - 1];
            RectTransform transform = obj.GetComponent<RectTransform>();

            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                transform.SetAsLastSibling();
            }
            else
            {
                if (lastWhich == which)
                {
                    obj.SetActive(false);
                    transform.SetAsFirstSibling();
                } else
                {
                    if (transform.GetSiblingIndex() != availableWindows.Length - 1)
                    {
                        transform.SetAsLastSibling();
                    } else
                    {
                        obj.SetActive(false);
                        transform.SetAsFirstSibling();
                    }
                }
            }

            lastWhich = which;
        }
        else
        {
            Debug.LogWarning("Window to toggle is not assigned.");
        }
    }
}
