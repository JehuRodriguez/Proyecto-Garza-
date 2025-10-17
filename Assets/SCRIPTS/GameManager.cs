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
}
