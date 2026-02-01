using System;
using System.Collections;
using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public int evidenceCollected { get; private set; }
    public int tasksCompleted { get; private set; }
    public int totalTasks { get; private set; }

    public float timeSinceBegin;
    public float timeMultiplier { get; private set; }
    public bool eventTime, moderatorPerms = true;

    public float eventCooldown = 10.0f;
    public float taskCompletion = 0.0f;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private FloatVariable sanity;
    [SerializeField] private float naturalSanityDecrese = 0.001f;

    [SerializeField] private AudioSource[] audioAmbience;
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioClip clickClip;
    private int currentSource = -1;

    [SerializeField] private EvidenceController evidenceCatcher;
    private bool policeAvailable = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
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
        GameOver,
        Win
    }

    void Start()
    {
        CurrentState = GameState.Intro;
        timeMultiplier = 1.0f;

        for (int i = 0; i < audioAmbience.Length; i++)
        {
            audioAmbience[i].volume = 0.0f;
        }

        ChangeAmbiance(0);
    }

    public void ChangeState(GameState newState)
    {
        switch (CurrentState)
        {
            case GameState.Intro:
                if (newState == GameState.Livestream || newState == GameState.LivestreamMax ||
                    newState == GameState.Evidence || newState == GameState.Notepad || newState == GameState.None)
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
                if(newState == GameState.Win)
                {
                    winScreen.SetActive(true);
                    ChangeAmbiance(4);
                }
                if(newState == GameState.GameOver)
                {
                    loseScreen.SetActive(true);
                    ChangeAmbiance(5);
                }
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

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if (clickSource && clickClip)
                {
                    clickSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
                    clickSource.PlayOneShot(clickClip);
                }
            }
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
            StartCoroutine(FadeOutCore(audioAmbience[currentSource], 0.5f));
        }
        StartCoroutine(FadeInCore(audioAmbience[which], 0.5f));
        currentSource = which;
    }

    private IEnumerator FadeOutCore(AudioSource src, float FadeTime)
    {
        float startVolume = src.volume;
        while (src.volume > 0f)
        {
            src.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        src.volume = 0f;
    }

    private IEnumerator FadeInCore(AudioSource src, float FadeTime)
    {
        while (src.volume < 1f)
        {
            src.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
        src.volume = 1f;
    }

    public void CallEvidenceCollect(int type = 0, String extraWords = "")
    {
        switch (type)
        {
            case 1:
                evidenceCatcher.createNewEvidence("Chat logs with " + extraWords, Color.navyBlue, 1);
                break;
            default:
                break;
        }

        if (policeAvailable == false && evidenceCollected >= 5) {
            evidenceCatcher.createNewEvidence("Call the police.", Color.blue, 911);
            policeAvailable = true;
        }
    }

}
