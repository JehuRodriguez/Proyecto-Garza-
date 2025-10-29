using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Tiempo")]
    public float startTime = 30f;
    public float currentTime = 30f;
    public float targetTime = 60f;

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
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
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
          
            timeText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }

    public void StartTimer()
    {
        gameFinished = false;
        IsRunning = true;
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

        SpawnerOneByOne sp = FindObjectOfType<SpawnerOneByOne>();
        if (sp != null) sp.StopSpawning();

        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = won ? "¡GANASTE!" : "GAME OVER";
        }
    }

}
