using UnityEngine;

public class MinimizeWindow : MonoBehaviour
{
    [SerializeField] private GameObject windowToToggle;

    public void DoMinimizeWindow()
    {
        if (windowToToggle != null)
        {
            windowToToggle.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Window to minimize is not assigned.");
        }
    }
}
