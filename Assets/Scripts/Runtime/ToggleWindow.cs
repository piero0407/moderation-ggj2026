using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleWindow : MonoBehaviour
{
    [SerializeField] private GameObject windowToToggle;

    public void ToggleWindowVisibility()
    {
        if (windowToToggle != null)
        {
            bool isActive = windowToToggle.activeSelf;
            windowToToggle.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("Window to toggle is not assigned.");
        }
    }
}
