using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Range(0f, 100f)]
    public float ecosystemHealth = 50f;

    public Action<float> OnHealthChanged;
    public Action OnDefeat;
    public Action OnVictory;

    

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
        Debug.Log("GameManager: VICTORIA triggered");
        
        if (PlayerLifeUI.Instance != null)
        {
            float life = PlayerLifeUI.Instance.GetCurrentLife();
            PlayerPrefs.SetFloat("LastPlayerLife", life);
            PlayerPrefs.Save();
        }

        OnVictory?.Invoke();
       
        SceneManager.LoadScene("Victory");
    }


    public void TriggerDefeat()
    {
        Debug.Log("GameManager: ¡Derrota activada!");
        OnDefeat?.Invoke();
    }

   
}

