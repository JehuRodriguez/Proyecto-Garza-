
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    public float startTime = 40f;
    public float currentTime = 40f;
    public float targetTime = 75f;

    [Header("Ajustes")]
    public bool runningOnStart = true;
    public bool clampToPositive = true;

    [Header("Aceleración por bloques de 10s")]
    [Tooltip("Cada 10 segundos transcurridos desde startTime, la cuenta se hará más rápida en este factor.")]
    public float speedStep = 0.20f;

    [Tooltip("Si tocas un aliado, se añade este número de 'pasos' extra (como si hubieran pasado X bloques de 10s).")]
    public int allyExtraSteps = 1;

    [Tooltip("Cuánto tiempo (s) dura el efecto de aceleración extra al tocar un aliado.")]
    public float allyAccelDuration = 6f;

    [Header("UI (TextMeshPro)")]
    public TextMeshProUGUI timeText;     
    public TextMeshProUGUI messageText;

    int allyExtraActiveSteps = 0;
    float allyAccelTimer = 0f;

    public bool IsRunning { get; private set; } = false;
    bool gameFinished = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            DestroyImmediate(gameObject);

        if (messageText != null && !Application.isPlaying)
            messageText.gameObject.SetActive(false);
    }

    void OnValidate()
    {
       
#if UNITY_EDITOR
        if (!Application.isPlaying && messageText != null)
        {
            messageText.gameObject.SetActive(false);
            EditorUtility.SetDirty(messageText);
        }
#endif
    }


    void Start()
    {
        if (!Application.isPlaying) return;

        currentTime = startTime;
        UpdateTimeUI();
        if (messageText != null)
            messageText.gameObject.SetActive(false);
        if (runningOnStart) StartTimer();
    }

    void Update()
    {
        if (!IsRunning || gameFinished) return;

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
            OnGameOver(false);
        else if (currentTime >= targetTime)
            OnGameOver(true);
    }

    void UpdateTimeUI()
    {
        if (timeText != null)
        {
          
            timeText.text = Mathf.CeilToInt(currentTime).ToString("00");
        }
    }

    public void StartTimer()
    {
        gameFinished = false;
        IsRunning = true;
        GameIsOver = false;
        allyExtraActiveSteps = 0;
        allyAccelTimer = 0f;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    public void AddTime(float seconds)
    {
        if (gameFinished) return;
        currentTime += seconds;
        UpdateTimeUI();

        if (currentTime >= targetTime) OnGameOver(true);
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
            messageText.gameObject.SetActive(true);
            messageText.text = won ? "¡GANASTE!" : "GAME OVER";
        }
    }

}
