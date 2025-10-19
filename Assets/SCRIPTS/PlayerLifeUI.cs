using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using TMPro;

public class PlayerLifeUI : MonoBehaviour
{
    public static PlayerLifeUI Instance;

    [Header("Configuración de vida")]
    public float maxPlayerLife = 100f;
    [Tooltip("Valor inicial de vida (0..maxPlayerLife). Si quieres empezar vacía, pon 0.")]
    public float startLife = 0f;

    [Header("UI")]
    public Slider lifeSlider;         
    public TextMeshProUGUI lifeText;   

    private float currentLife;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        currentLife = Mathf.Clamp(startLife, 0f, maxPlayerLife);
        UpdateUIImmediate();
    }

    void UpdateUIImmediate()
    {
        if (lifeSlider != null)
        {
            lifeSlider.maxValue = maxPlayerLife;
            lifeSlider.value = currentLife;
        }

        if (lifeText != null)
        {
            lifeText.text = $"{Mathf.RoundToInt(currentLife)} / {Mathf.RoundToInt(maxPlayerLife)}";
        }
    }

    public void AddLife(float amount)
    {
        if (amount <= 0) return;
        currentLife = Mathf.Clamp(currentLife + amount, 0f, maxPlayerLife);
        UpdateUIImmediate();

        if (Mathf.Approximately(currentLife, maxPlayerLife) || currentLife >= maxPlayerLife)
        {
            Debug.Log("GANASTE");
        }
    }

    public void RemoveLife(float amount)
    {
        if (amount <= 0) return;
        currentLife = Mathf.Clamp(currentLife - amount, 0f, maxPlayerLife);
        UpdateUIImmediate();
    }

    public void SetLife(float value)
    {
        currentLife = Mathf.Clamp(value, 0f, maxPlayerLife);
        UpdateUIImmediate();
    }

}
