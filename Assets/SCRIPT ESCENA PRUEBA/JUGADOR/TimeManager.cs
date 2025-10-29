
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("UI (TextMeshPro)")]
    public TextMeshProUGUI timeText;     
    public TextMeshProUGUI messageText;

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

       
        currentTime -= Time.deltaTime;
        if (clampToPositive && currentTime < 0f) currentTime = 0f;

        UpdateTimeUI();

        if (currentTime <= 0f)
        {
            
            OnGameOver(false);
        }
        else if (currentTime >= targetTime)
        {
           
            OnGameOver(true);
        }
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
