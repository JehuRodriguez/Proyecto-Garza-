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
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void OnTimerEnd()
    {
        Debug.Log("¡El tiempo se acabó!");
        FindObjectOfType<Spawner>().StopSpawning();
    }

}
