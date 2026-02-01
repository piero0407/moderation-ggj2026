using UnityEngine;
using UnityEngine.UI;

public class LivestreamLayoutManager : MonoBehaviour
{
    [Header("References")]
    public RectTransform chatBox;
    public RectTransform cameraView;
    public RectTransform stats;

    [Header("Settings")]
    public float chatWidth = 340f;

    private Vector2 defaultCameraAnchorMin;
    private Vector2 defaultCameraAnchorMax;
    private Vector2 defaultCameraSizeDelta;
    private Vector2 defaultCameraPosition;

    [SerializeField] private Button cameraButton;
    [SerializeField] private Image hideChatImage;

    [SerializeField] private Sprite chatHidden, chatVisible;

    void Awake()
    {
        defaultCameraAnchorMin = cameraView.anchorMin;
        defaultCameraAnchorMax = cameraView.anchorMax;
        defaultCameraSizeDelta = cameraView.sizeDelta;
        defaultCameraPosition = cameraView.anchoredPosition;

        cameraButton.interactable = cameraButton.enabled = false;
        hideChatImage.sprite = chatVisible;
    }

    public void ToggleChat()
    {
        bool chatOn = chatBox.gameObject.activeSelf;

        if (chatOn)
        {
            chatBox.gameObject.SetActive(false);
            stats.offsetMax = new Vector2(0, stats.offsetMax.y);

            float currentWidth = cameraView.rect.width;

            cameraView.anchorMin = new Vector2(0.5f, defaultCameraAnchorMin.y);
            cameraView.anchorMax = new Vector2(0.5f, defaultCameraAnchorMax.y);
            cameraView.sizeDelta = new Vector2(currentWidth, defaultCameraSizeDelta.y);

            float pivotCorrection = (0.5f - cameraView.pivot.x) * currentWidth;
            cameraView.anchoredPosition = new Vector2(-pivotCorrection, defaultCameraPosition.y);

            cameraButton.interactable = cameraButton.enabled = true;
            hideChatImage.sprite = chatHidden;
        }
        else
        {
            chatBox.gameObject.SetActive(true);
            stats.offsetMax = new Vector2(-chatWidth, stats.offsetMax.y);

            cameraView.anchorMin = defaultCameraAnchorMin;
            cameraView.anchorMax = defaultCameraAnchorMax;
            cameraView.sizeDelta = defaultCameraSizeDelta;

            cameraView.anchoredPosition = defaultCameraPosition;

            cameraButton.interactable = cameraButton.enabled = false;
            hideChatImage.sprite = chatVisible;
        }
    }
}