using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public static bool GameIsOver = false;

    public float startTime = 60f;
    public float currentTime = 60f;
    public float targetTime = 100f;

    public bool runningOnStart = true;
    public bool clampToPositive = true;
    public bool clampToMax = true;

    public float speedStep = 0.20f;
    public int allyExtraSteps = 1;
    public float allyAccelDuration = 6f;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI messageText;

    int allyExtraActiveSteps = 0;
    float allyAccelTimer = 0f;
    bool gameFinished = false;

    public bool IsRunning { get; private set; } = false;
    float playTimer = 0f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) { Destroy(gameObject); return; }
    }

    void Start()
    {
        currentTime = startTime;
        playTimer = 0f;
        if (messageText != null) messageText.gameObject.SetActive(false);
        if (runningOnStart) StartTimer();
        Debug.Log("[TimeManager] Simple START");
    }

    void Update()
    {
        if (!IsRunning || gameFinished) return;
        playTimer += Time.deltaTime;

        float timePassed = Mathf.Max(0f, startTime - currentTime);
        int stepsFromTime = Mathf.FloorToInt(timePassed / 10f);
        float speedMultiplier = 1f + Mathf.Max(0, stepsFromTime) * speedStep;

        currentTime -= Time.deltaTime * speedMultiplier;

        if (clampToPositive && currentTime < 0f) currentTime = 0f;
        if (clampToMax && currentTime > targetTime) currentTime = targetTime;

        if (timeText != null) timeText.text = Mathf.CeilToInt(currentTime).ToString("00");

        if (currentTime <= 0f)
        {
            Debug.Log("[TimeManager] Update -> currentTime <= 0 -> OnGameOver(false)");
            OnGameOver(false);
            return;
        }

        if (currentTime >= targetTime)
        {
            Debug.Log("[TimeManager] Update -> currentTime >= targetTime -> OnGameOver(true). currentTime=" + currentTime);
            OnGameOver(true);
            return;
        }
    }

    public void StartTimer()
    {
        gameFinished = false;
        IsRunning = true;
        GameIsOver = false;
        allyExtraActiveSteps = 0;
        allyAccelTimer = 0f;
        playTimer = 0f;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void AddTime(float seconds)
    {
        if (gameFinished) return;
        currentTime += seconds;
        if (clampToMax && currentTime > targetTime) currentTime = targetTime;
        if (timeText != null) timeText.text = Mathf.CeilToInt(currentTime).ToString("00");

        if (currentTime >= targetTime)
        {
            Debug.Log("[TimeManager] AddTime alcanzó target -> llamando OnGameOver(true)");
            OnGameOver(true);
        }
    }

    public void SubtractTime(float seconds)
    {
        if (gameFinished) return;
        currentTime -= seconds;
        allyExtraActiveSteps += allyExtraSteps;
        allyAccelTimer = allyAccelDuration;
        if (clampToPositive && currentTime < 0f) currentTime = 0f;
        if (timeText != null) timeText.text = Mathf.CeilToInt(currentTime).ToString("00");
        if (currentTime <= 0f) OnGameOver(false);
    }

    void OnGameOver(bool won)
    {
        if (gameFinished) return;
        gameFinished = true;
        IsRunning = false;
        GameIsOver = true;

        SpawnerOneByOne sp = FindObjectOfType<SpawnerOneByOne>();
        if (sp != null) sp.StopSpawning();

        
        if (messageText != null) messageText.gameObject.SetActive(false);

        
        int finalScore = Mathf.RoundToInt(playTimer);

        string playerName = PlayerPrefs.HasKey("PlayerName") ? PlayerPrefs.GetString("PlayerName") : "Jugador";

        
        MyGame.Profiles.SimpleManager.Instance?.AddAttempt(playerName, finalScore);

       
        if (SimpleUIManager.Instance != null)
        {
            SimpleUIManager.Instance.HandleGameOver(won, finalScore);
        }
        else
        {
            Debug.LogError("[TimeManager] SimpleUIManager.Instance es null -> no puedo mostrar panel.");
        }

        Debug.Log("[TimeManager] OnGameOver won=" + won + " finalScore=" + finalScore);

    }

}


