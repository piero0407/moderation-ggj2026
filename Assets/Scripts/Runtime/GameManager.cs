using System.Collections;
using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public int evidenceCollected { get; private set; }
    public int tasksCompleted { get; private set; }
    public int totalTasks { get; private set; }

    public float timeSinceBegin;
    public float timeMultiplier { get; private set; }
    public bool eventTime;

    public float eventCooldown = 10.0f;
    public float taskCompletion = 0.0f;

    [SerializeField] private FloatVariable sanity;
    [SerializeField] private float naturalSanityDecrese = 0.001f;

    [SerializeField] private AudioSource[] audioAmbience;
    private int currentSource = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        for (int i = 0; i < audioAmbience.Length; i++)
        {
            audioAmbience[i].volume = 0.0f;
        }

        ChangeAmbiance(0);
    }
    public enum GameState
    {
        Intro,
        Start,
        Livestream,
        LivestreamMax,
        Notepad,
        Evidence,
        None,
        Paused,
        GameOver
    }

    void Start()
    {
        CurrentState = GameState.Intro;
        timeMultiplier = 1.0f;
    }

    public void ChangeState(GameState newState)
    {
        switch (CurrentState)
        {
            case GameState.Intro:
                if (newState == GameState.Livestream)
                    CurrentState = GameState.Start;
                break;
            case GameState.Evidence:
            case GameState.Notepad:
            case GameState.Livestream:
            case GameState.LivestreamMax:
            case GameState.None:
                CurrentState = newState;
                break;
            default:
                CurrentState = newState;
                break;
        }
    }

    void Update()
    {
        switch (CurrentState)
        {
            case GameState.Evidence:
            case GameState.Notepad:
            case GameState.Livestream:
            case GameState.LivestreamMax:
            case GameState.None:
                sanity.Value -= Time.deltaTime * naturalSanityDecrese;
                break;

            default:
                break;
        }


    }

    public void IncreaseComplexity()
    {
        timeMultiplier += 0.15f;
        eventTime = false;
        eventCooldown = 10.0f;

        totalTasks += 1;
        if (taskCompletion > 0.85f) tasksCompleted += 1;
        taskCompletion = 0.0f;
    }

    public void ChangeAmbiance(int which)
    {
        if (which > audioAmbience.Length - 1) return;
        if (currentSource >= 0)
        {
            IEnumerator fadeSound1 = FadeOut(audioAmbience[currentSource], 0.5f);
            StartCoroutine(fadeSound1);
        }
        IEnumerator fadeSound2 = FadeIn(audioAmbience[which], 0.5f);
        StartCoroutine(fadeSound2);
        currentSource = which;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
