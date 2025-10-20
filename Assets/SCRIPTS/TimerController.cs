using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class TimerController : MonoBehaviour
{
    public float maxTime = 90f; 
    private float currentTime;
    public TextMeshProUGUI timerText; 
    private bool timerActive = true;
    public Spawner spawner;

    void Start()
    {
        currentTime = maxTime;
        UpdateTimerText();
    }

    void Update()
    {
        if (!timerActive) return;

        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            currentTime = 0;
            timerActive = false;
            OnTimerEnd();
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void OnTimerEnd()
    {
        Debug.Log("¡El tiempo se acabó!");
        if (spawner != null) spawner.StopSpawning();
        else
        {
            Spawner s = FindObjectOfType<Spawner>();
            if (s != null) s.StopSpawning();
        }


        if (GameManager.Instance != null) GameManager.Instance.TriggerDefeat();
        else
        {
            
            Time.timeScale = 0f;
        }
    }

    public void ResetAndStart()
    {
        Time.timeScale = 1f;
        currentTime = maxTime;
        timerActive = true;
    }

}
