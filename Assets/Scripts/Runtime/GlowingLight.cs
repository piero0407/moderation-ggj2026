using UnityEngine;
using UnityEngine.UI;

public class GlowingLight : MonoBehaviour
{
    [Header("Intensity Settings")]
    public float minBrightness = 0.5f;
    public float maxBrightness = 1.0f;

    [Header("Animation Settings")]
    public float pulseSpeed = 2.0f;
    public float flickerSpeed = 15.0f;

    [Header("Style Mixing")]
    [Range(0f, 1f)]
    public float flickerBias = 0.2f;

    private RawImage rawImage;
    private Color originalColor;

    void Start()
    {
        rawImage = GetComponent<RawImage>();

        if (rawImage != null) originalColor = rawImage.color;
    }

    void Update()
    {
        if (rawImage == null) return;

        float smoothPulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float erraticFlicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.0f);
        float currentSignal = Mathf.Lerp(smoothPulse, erraticFlicker, flickerBias);
        float finalIntensity = Mathf.Lerp(minBrightness, maxBrightness, currentSignal);

        rawImage.color = new Color(
            originalColor.r * finalIntensity,
            originalColor.g * finalIntensity,
            originalColor.b * finalIntensity,
            originalColor.a
        );
    }
}
