using System.Diagnostics.CodeAnalysis;
using System.Threading;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField] private float eventCooldown = 8.0f;
    [SerializeField] private float taskProgress = 0.0f; // from 0 to 1
    public float timeMultiplier = 1.0f;
    public bool eventTime { get; private set; } = false;

    void Update()
    {
        if (eventCooldown >= 0.0f)
        {
            if (eventTime) eventTime = false;
            eventCooldown -= Time.deltaTime * timeMultiplier;
        }
        else
        {
            eventCooldown = 8.0f;
            Debug.Log("TIME FOR AN EVENT.");
            eventTime = true;
        }
    }
}
