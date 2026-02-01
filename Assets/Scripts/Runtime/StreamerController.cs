using System;
using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;
using UnityEngine.UI;

public class StreamerController : MonoBehaviour
{
    [SerializeField] private CharacterExpressions characterExpressions;
    [SerializeField] Image baseSprite;
    [SerializeField] Image faceSprite;
    [SerializeField] FloatVariable sanity;
    [SerializeField, Range(0, 7)] int expression;

    void OnValidate()
    {
        baseSprite.sprite = characterExpressions.Expressions[expression].bodySprite;
        faceSprite.sprite = characterExpressions.Expressions[expression].faceSprite;
    }
    void Start()
    {
        baseSprite.sprite = characterExpressions.Expressions[expression].bodySprite;
        faceSprite.sprite = characterExpressions.Expressions[expression].faceSprite;
    }

    void Update()
    {
        expression = Mathf.Clamp((int)((sanity.Value * -1 + 1) * 6), 0, 6);
        baseSprite.sprite = characterExpressions.Expressions[expression].bodySprite;
        faceSprite.sprite = characterExpressions.Expressions[expression].faceSprite;
    }
}
