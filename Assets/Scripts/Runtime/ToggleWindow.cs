using System.Linq;
using UnityEngine;

public class ToggleWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] possibleWindows;
    private int lastWhich = 0;

    public void ToggleWindowVisibility(int which)
    {
        if (which != 0)
        {
            GameObject obj = possibleWindows[which - 1];

            RectTransform rt = obj.GetComponent<RectTransform>();
            if (rt.GetSiblingIndex() == possibleWindows.Length - 1)
            {
                rt.GetComponent<RectTransform>().SetAsFirstSibling();
                obj.SetActive(false);
            } else if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }

            if (obj.activeSelf) rt.SetAsLastSibling();
            lastWhich = which;
        }
        else
        {
            Debug.LogWarning("Window to toggle is not assigned.");
        }
    }
}
