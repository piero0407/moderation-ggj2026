using System.Collections;
using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;
using UnityEngine.UI;

public class StreamerController : MonoBehaviour
{
    [SerializeField] private CharacterExpressions characterExpressions;
    [SerializeField] private Image baseSprite;
    [SerializeField] private Image faceSprite;
    [SerializeField] private FloatVariable sanity;
    [SerializeField] private AudioClip[] orderedEmotions;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private float jumpDuration = 0.2f;
    [SerializeField] private float breathSpeed = 1.0f;
    [SerializeField] private float breathIntensity = 0.03f; 

    private int currentExpressionIndex;
    private int lastExpressionIndex = -1;
    private float gruntTimer = 10.0f;
    private Vector3 originalPositionBody, originalPositionFace;
    private Vector3 originalScaleBody, originalScaleFace;
    private Coroutine jumpCoroutine;

    void OnValidate()
    {
        if (characterExpressions != null && baseSprite != null && faceSprite != null && characterExpressions.Expressions.Count > 0)
        {
            int tempExpression = 0;
            if (sanity != null)
                tempExpression = Mathf.Clamp((int)((sanity.Value * -1 + 1) * 6), 0, 6);

            if (tempExpression < characterExpressions.Expressions.Count)
            {
                baseSprite.sprite = characterExpressions.Expressions[tempExpression].bodySprite;
                faceSprite.sprite = characterExpressions.Expressions[tempExpression].faceSprite;
            }
        }
    }

    void Start()
    {
        if (baseSprite != null)
        {
            originalPositionBody = baseSprite.rectTransform.anchoredPosition;
            originalScaleBody = baseSprite.rectTransform.localScale;
        }
        if (faceSprite != null)
        {
            originalPositionFace = faceSprite.rectTransform.anchoredPosition;
            originalScaleFace = faceSprite.rectTransform.localScale;
        }

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        UpdateExpressionLogic();
    }

    void Update()
    {
        UpdateExpressionLogic();
        HandleBreathing();
        HandleAudio();
    }

    private void HandleBreathing()
    {
        float breathCycle = Mathf.Sin(Time.time * breathSpeed);
        float scaleY = 1.0f + (breathCycle * breathIntensity);

        Vector3 newScaleBody = originalScaleBody;
        newScaleBody.y *= scaleY;
        
        Vector3 newScaleFace = originalScaleFace;
        newScaleFace.y *= scaleY;

        baseSprite.rectTransform.localScale = newScaleBody;
        faceSprite.rectTransform.localScale = newScaleFace;

        baseSprite.rectTransform.anchoredPosition = originalPositionBody + new Vector3(0, scaleY, 0);
        faceSprite.rectTransform.anchoredPosition = originalPositionFace + new Vector3(0, scaleY, 0);
    }

    private void UpdateExpressionLogic()
    {
        currentExpressionIndex = Mathf.Clamp((int)((sanity.Value * -1 + 1) * 6), 0, 6);

        if (currentExpressionIndex != lastExpressionIndex)
        {
            UpdateVisuals(currentExpressionIndex);
            TriggerJump();
            lastExpressionIndex = currentExpressionIndex;
        }
    }

    private void UpdateVisuals(int index)
    {
        if (index >= 0 && index < characterExpressions.Expressions.Count)
        {
            baseSprite.sprite = characterExpressions.Expressions[index].bodySprite;
            faceSprite.sprite = characterExpressions.Expressions[index].faceSprite;
        }
    }

    private void TriggerJump()
    {
        if (jumpCoroutine != null) StopCoroutine(jumpCoroutine);
        jumpCoroutine = StartCoroutine(JumpAnimation());
    }

    private IEnumerator JumpAnimation()
    {
        float elapsed = 0f;
        RectTransform rtBody = baseSprite.rectTransform;
        RectTransform rtFace = faceSprite.rectTransform;

        rtBody.anchoredPosition = originalPositionBody;
        rtFace.anchoredPosition = originalPositionFace;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;
            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;

            rtBody.anchoredPosition = originalPositionBody + new Vector3(0, yOffset, 0);
            rtFace.anchoredPosition = originalPositionFace + new Vector3(0, yOffset, 0);
            yield return null;
        }

        rtBody.anchoredPosition = originalPositionBody;
        rtFace.anchoredPosition = originalPositionFace;
        jumpCoroutine = null;
    }

    private void HandleAudio()
    {
        if (!audioSource.isPlaying)
        {
            if (gruntTimer > 0.0f)
            {
                gruntTimer -= Time.deltaTime;
            }
            else
            {
                gruntTimer = UnityEngine.Random.Range(10.0f, 15.0f);

                if (currentExpressionIndex < orderedEmotions.Length)
                {
                    audioSource.clip = orderedEmotions[currentExpressionIndex];
                    audioSource.Play();
                    if (currentExpressionIndex < orderedEmotions.Length - 1) TriggerJump();
                }
            }
        }
    }
}