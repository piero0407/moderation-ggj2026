using UnityEngine;
using UnityEngine.UI;

public class TaskbarFlash : MonoBehaviour
{
    [Header("refs")]
    public Image targetImage;
    public RectTransform windowRect; 
    public float velocidad = 5f;
    public Color originalColor = Color.white;
    public Color highColor = new Color(0.6f, 0.6f, 0.6f, 1f); 

    private bool activated = false;

    void Update()
    {
        if (activated && windowRect != null)
        {
            bool isOnTop = windowRect.GetSiblingIndex() == windowRect.parent.childCount - 1;
            
            if (isOnTop && windowRect.gameObject.activeSelf)
            {
                SetFlashing(false);
            }
        }

        if (activated && targetImage != null)
        {
            float t = (Mathf.Sin(Time.time * velocidad) + 1f) / 2f;
            targetImage.color = Color.Lerp(originalColor, highColor, t);
        }
    }

    public void CallForAttention()
    {
        if (windowRect == null) return;

        bool isOnTop = windowRect.GetSiblingIndex() == windowRect.parent.childCount - 1;

        if (!isOnTop || !windowRect.gameObject.activeSelf)
        {
            SetFlashing(true);
        }
    }

    public void SetFlashing(bool state)
    {
        activated = state;
        if (!state && targetImage != null) 
        {
            targetImage.color = originalColor;
        }
    }
}