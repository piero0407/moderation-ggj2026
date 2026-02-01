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
                if (obj.name == "Livestream") obj.BroadcastMessage("DisableTaskAction", true); 
            }
            else
            {
                if (lastWhich == which)
                {
                    obj.SetActive(false);
                    transform.SetAsFirstSibling();
                    if (obj.name == "Livestream") obj.BroadcastMessage("DisableTaskAction", false); 
                } else
                {
                    if (transform.GetSiblingIndex() != availableWindows.Length - 1)
                    {
                        transform.SetAsLastSibling();
                        if (obj.name == "Livestream") obj.BroadcastMessage("DisableTaskAction", true); 
                    } else
                    {
                        obj.SetActive(false);
                        transform.SetAsFirstSibling();
                        if (obj.name == "Livestream") obj.BroadcastMessage("DisableTaskAction", false); 
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
