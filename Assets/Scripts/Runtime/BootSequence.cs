using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(AudioSource))]
public class BootSequence : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float waitTime = 2.0f; 
    [SerializeField] private float fadeDuration = 1.0f;

    private static bool hasBooted = false;

    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator Start()
    {
        yield return null; 

        if (hasBooted)
        {
            GameManager.Instance.ChangeAmbiance(0);
            gameObject.SetActive(false);
        }
        else
        {
            hasBooted = true;
            StartCoroutine(PlaySequence());
        }
    }

    private IEnumerator PlaySequence()
    {
        canvasGroup.alpha = 1f;
        if (audioSource.clip != null) audioSource.Play();
        
        yield return new WaitForSeconds(audioSource.clip.length - 6.0f);

        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        GameManager.Instance.ChangeAmbiance(0);
        gameObject.SetActive(false);
    }
}