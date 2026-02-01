using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using TMPro;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField] private float eventCooldown = 8.0f;
    [SerializeField] private float taskProgress = 0.0f; // from 0 to 1
    [SerializeField] private float timeSinceBegin = 0.0f;

    [SerializeField] private TMP_Text gameStatus;
    [SerializeField] private TMP_Text taskLabel;
    public float timeMultiplier = 1.0f;
    public bool eventTime { get; private set; } = false;

    void Update()
    {
        timeSinceBegin += Time.deltaTime;

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

        TimeSpan t = TimeSpan.FromSeconds(timeSinceBegin);
        gameStatus.text = $"Time multiplier: {timeMultiplier}x\nTime passed: {string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds)}\nEvidence collected: {GameManager.Instance.evidenceCollected}";
    }
}
