using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Expression
{
    [SerializeField] public string name;
    [SerializeField] public Sprite bodySprite;
    [SerializeField] public Sprite faceSprite;
}

[CreateAssetMenu(fileName = "CharacterExpressions", menuName = "Scriptable Objects/CharacterExpressions")]
public class CharacterExpressions : ScriptableObject
{
    [SerializeField] private List<Expression> expressions;
    public List<Expression> Expressions { get => expressions; }
}