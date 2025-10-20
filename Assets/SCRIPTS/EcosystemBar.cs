using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcosystemBar : MonoBehaviour
{
    public static EcosystemBar Instance;

    public Slider ecosystemSlider;
    public float maxValue = 100f;
    private float currentValue = 50f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (ecosystemSlider != null)
        {
            ecosystemSlider.minValue = 0f;
            ecosystemSlider.maxValue = maxValue;
            ecosystemSlider.value = currentValue;
        }
    }

    public void AddValue(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0f, maxValue);
        if (ecosystemSlider != null)
            ecosystemSlider.value = currentValue;
    }
}
