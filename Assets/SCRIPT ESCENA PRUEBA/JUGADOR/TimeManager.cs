using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public static bool GameIsOver = false;

    [Header("Tiempo")]
    public float startTime = 60f;
    public float currentTime = 60f;
    public float targetTime = 100f;

    [Header("Ajustes")]
    public bool runningOnStart = true;
    public bool clampToPositive = true;
    public bool clampToMax = true;

    [Header("Aceleración por bloques de 10s")]
    public float speedStep = 0.20f;
    public int allyExtraSteps = 1;
    public float allyAccelDuration = 6f;

    [Header("UI (TextMeshPro)")]
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
        else if (Instance != this) { Debug.LogWarning("[TimeManager] Instancia duplicada destruida."); Destroy(gameObject); return; }
    }


    void Start()
    {
        currentTime = startTime;
        playTimer = 0f;
        UpdateTimeUI();
        if (messageText != null) messageText.gameObject.SetActive(false);
        if (runningOnStart) StartTimer();
        Debug.Log("[TimeManager] Start - startTime=" + startTime + " targetTime=" + targetTime);
    }

    void Update()
    {
        if (!IsRunning || gameFinished) return;

        playTimer += Time.deltaTime;

        if (allyExtraActiveSteps > 0)
        {
            allyAccelTimer -= Time.deltaTime;
            if (allyAccelTimer <= 0f)
            {
                allyExtraActiveSteps = 0;
                allyAccelTimer = 0f;
            }
        }

        float timePassed = Mathf.Max(0f, startTime - currentTime);
        int stepsFromTime = Mathf.FloorToInt(timePassed / 10f);
        if (stepsFromTime < 0) stepsFromTime = 0;

        float speedMultiplier = 1f + stepsFromTime * speedStep + allyExtraActiveSteps * speedStep;

        currentTime -= Time.deltaTime * speedMultiplier;

        if (clampToPositive && currentTime < 0f) currentTime = 0f;


        UpdateTimeUI();

        if (currentTime <= 0f)
        {
            OnGameOver(true);
            return;
        }

        if (clampToMax && currentTime >= targetTime)
        {
            OnGameOver(true);
            return;
        }

    }

    void UpdateTimeUI()
    {
        if (timeText != null)
            timeText.text = Mathf.CeilToInt(currentTime).ToString("00");
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
        UpdateTimeUI();
        if (clampToMax && currentTime >= targetTime)
        {
            Debug.Log("[TimeManager] AddTime alcanzó target -> GANÓ");
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
        UpdateTimeUI();

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

        if (messageText != null)
        {
            messageText.text = won ? "¡GANASTE!" : "PERDISTE";
            messageText.gameObject.SetActive(true);
        }


        int finalScore = Mathf.RoundToInt(playTimer);

        string playerName = PlayerPrefs.HasKey("PlayerName") ? PlayerPrefs.GetString("PlayerName") : "Jugador";
        

        MyGame.Profiles.SimpleManager.Instance?.AddAttempt(playerName, finalScore);
        if (SimpleUIManager.Instance != null) SimpleUIManager.Instance.HandleGameOver(won, finalScore);
        else Debug.LogError("[TimeManager] SimpleUIManager.Instance es null -> no puedo mostrar panel.");
    }

}


