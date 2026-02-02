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

    private Coroutine[] ambianceCoroutines;
    [SerializeField] private AudioSource[] audioAmbience;
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip screamClip;
    private int currentSource = -1;

    [SerializeField] private EvidenceController evidenceCatcher;
    private bool policeAvailable = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        if (audioAmbience != null)
            ambianceCoroutines = new Coroutine[audioAmbience.Length];
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
    }

    public void ChangeState(GameState newState)
    {
        switch (CurrentState)
        {
            case GameState.Intro:
                if (newState == GameState.Livestream || newState == GameState.LivestreamMax ||
                    newState == GameState.Evidence || newState == GameState.Notepad || newState == GameState.None)
                {
                    CurrentState = GameState.Start;
                }
                else
                {
                    CurrentState = newState;
                }
                break;
            default:
                CurrentState = newState;
                break;
        }

        if (CurrentState == GameState.Win)
        {
            Debug.Log($"Attempting to show Win Screen. Ref is null? {winScreen == null}");

            if (winScreen)
            {
                winScreen.SetActive(true);
                winScreen.transform.SetAsLastSibling();
            }
            ChangeAmbiance(4);
        }
        else if (CurrentState == GameState.GameOver)
        {
            if (loseScreen) loseScreen.SetActive(true);
            ChangeAmbiance(5);

            if (clickSource && screamClip)
            {
                clickSource.pitch = 1.0f;
                clickSource.PlayOneShot(screamClip);
            }
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
                if (sanity.Value <= 0.0f)
                {
                    ChangeState(GameState.GameOver);
                }
                break;
            default:
                break;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (CurrentState != GameState.Win && CurrentState != GameState.GameOver)
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
        if (which < 0 || which >= audioAmbience.Length) return;
        if (currentSource == which && which != 4 && which != 5) return;
        if (currentSource >= 0 && currentSource < audioAmbience.Length && currentSource != which)
        {
            FadeAudio(currentSource, 0f, 0.5f);
        }

        AudioSource newSource = audioAmbience[which];
        bool isOneShot = (which == 4 || which == 5);

        if (isOneShot)
        {
            newSource.loop = false;
            newSource.time = 0f;
            newSource.Play();
            FadeAudio(which, 1f, 0f);
        }
        else
        {
            newSource.loop = true;
            if (!newSource.isPlaying)
                newSource.Play();
            FadeAudio(which, 1f, 0.5f);
        }

        currentSource = which;
    }

    private void FadeAudio(int index, float targetVolume, float duration)
    {
        if (ambianceCoroutines == null) return;
        if (ambianceCoroutines[index] != null)
        {
            StopCoroutine(ambianceCoroutines[index]);
        }
        ambianceCoroutines[index] = StartCoroutine(FadeTo(audioAmbience[index], targetVolume, duration));
    }

    private IEnumerator FadeTo(AudioSource src, float targetVolume, float duration)
    {
        if (duration <= 0f)
        {
            src.volume = targetVolume;
            yield break;
        }

        float startVolume = src.volume;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            src.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        src.volume = targetVolume;
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

        evidenceCollected++;

        if (policeAvailable == false && evidenceCollected >= 5)
        {
            evidenceCatcher.createNewEvidence("Call the police.", Color.blue, 911);
            policeAvailable = true;
        }
    }

}
