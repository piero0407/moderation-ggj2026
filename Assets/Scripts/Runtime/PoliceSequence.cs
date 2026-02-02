using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PoliceSequence : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RawImage flashOverlay; 
    [SerializeField] private Color colorA = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private Color colorB = new Color(0f, 0f, 1f, 0.3f);

    [Header("Audio")]
    [SerializeField] private AudioSource sirenSource;

    [Header("Settings")]
    [SerializeField] private float flashSpeed = 10f;
    [SerializeField] private float fadeDuration = 2.0f;

    private void Start()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        float waitDuration = 4.0f;
        float startVolume = 1.0f;

        if (sirenSource)
        {
            sirenSource.volume = 1.0f;
            sirenSource.Play();
            startVolume = sirenSource.volume;
            
            if (sirenSource.clip != null)
            {
                waitDuration = sirenSource.clip.length / 2.0f;
            }
        }

        float timer = 0f;
        while (timer < waitDuration)
        {
            timer += Time.deltaTime;
            AnimateLights();
            yield return null;
        }

        float fadeTimer = 0f;
        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            AnimateLights();

            if (sirenSource)
            {
                sirenSource.volume = Mathf.Lerp(startVolume, 0f, fadeTimer / fadeDuration);
            }

            yield return null;
        }

        if (sirenSource) sirenSource.Stop();

        GameManager.Instance.ChangeState(GameManager.GameState.Win);
        gameObject.SetActive(false); 
    }

    private void AnimateLights()
    {
        if (flashOverlay)
        {
            float t = Mathf.PingPong(Time.time * flashSpeed, 1f);
            flashOverlay.color = Color.Lerp(colorA, colorB, t);
        }
    }
}