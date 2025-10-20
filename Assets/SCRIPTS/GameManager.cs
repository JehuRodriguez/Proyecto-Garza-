using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Range(0f, 100f)]
    public float ecosystemHealth = 50f;

    public Action<float> OnHealthChanged;
    public Action OnDefeat;
    public Action OnVictory;

    private bool finished = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ChangeHealth(float delta)
    {
        ecosystemHealth = Mathf.Clamp(ecosystemHealth + delta, 0f, 100f);
        OnHealthChanged?.Invoke(ecosystemHealth);
    }

    public void TriggerVictory()
    {
        if (finished) return;
        finished = true;
        Debug.Log("GameManager: VICTORIA");

        
        Spawner s = FindObjectOfType<Spawner>();
        if (s != null) s.StopSpawning();

       
        Time.timeScale = 0f;

        
        OnVictory?.Invoke();
    }


    public void TriggerDefeat()
    {
        if (finished) return;
        finished = true;
        Debug.Log("GameManager: DERROTA");

        Spawner s = FindObjectOfType<Spawner>();
        if (s != null) s.StopSpawning();

        Time.timeScale = 0f;

        OnDefeat?.Invoke();
    }

    public void ResetGame()
    {
        finished = false;
        Time.timeScale = 1f;
    }
}

