using System;
using TMPro;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField] private TMP_Text gameStatus;
    [SerializeField] private TMP_Text taskLabel;

    void Update()
    {
        GameManager gm = GameManager.Instance;

        if (gm.CurrentState == GameManager.GameState.Paused || gm.CurrentState == GameManager.GameState.GameOver) return;

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
        gameStatus.text = $"Time multiplier: {gm.timeMultiplier}x\nTime passed: {string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds)}\nEvidence collected: {GameManager.Instance.evidenceCollected}";
        taskLabel.text = $"Tasks done: {gm.tasksCompleted} | Total tasks recieved: {gm.totalTasks}";

        if (gm.eventCooldown > 0.0f)
        {
            gameStatus.text += $"\nA new task will begin in {(int)Math.Round(gm.eventCooldown)} seconds.";
        } else
        {
            if (gm.eventTime)
            {
                gameStatus.text += $"\nComplete your task.";
                taskLabel.text += $" | Task progress: {Math.Round(gm.taskCompletion * 100f)}%";
            }
        }
    }
}
