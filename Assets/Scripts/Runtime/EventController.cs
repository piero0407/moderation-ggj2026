using System;
using TMPro;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField] private TMP_Text timeClock;
    [SerializeField] private TMP_Text taskLabel;

    void Update()
    {
        GameManager gm = GameManager.Instance;

        if (gm.CurrentState == GameManager.GameState.Paused || gm.CurrentState == GameManager.GameState.GameOver || gm.CurrentState == GameManager.GameState.Win) return;

        gm.timeSinceBegin += Time.deltaTime;

        if (gm.eventCooldown > 0.0f)
        {
            if (gm.CurrentState != GameManager.GameState.Start && gm.CurrentState != GameManager.GameState.Intro) gm.eventCooldown -= Time.deltaTime * gm.timeMultiplier;
        }
        else
        {
            if (!gm.eventTime)
            {
                Debug.Log("TIME FOR AN EVENT.");
                gm.eventTime = true;
            }
        }

        TimeSpan t = TimeSpan.FromSeconds(GameManager.Instance.timeSinceBegin);
        timeClock.text = $"{string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds)}";
        taskLabel.text = $"Tareas completadas: {gm.tasksCompleted}";

        if (gm.eventCooldown <= 0.0f)
        {
            if (gm.eventTime)
            {
                taskLabel.text += $" | Task progress: {Math.Round(gm.taskCompletion * 100f)}%";
            }
        }
    }
}
