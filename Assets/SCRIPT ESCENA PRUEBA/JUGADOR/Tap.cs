using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap : MonoBehaviour
{
    public bool isEnemy = true;               
    public GameObject destroyVfxPrefab;       
    public float destroyDelay = 0f;

    [Header("Tiempo que cambia al ser tappeado")]
    public float enemyAddSeconds = 1f;   
    public float allySubtractSeconds = 2f; 


    public void OnTapped()
    {
        if (TimeManager.Instance != null)
        {
            if (isEnemy)
            {
                TimeManager.Instance.AddTime(enemyAddSeconds);
                TimeManager.Instance.AddScore(1); 
            }
            else
            {
                TimeManager.Instance.SubtractTime(allySubtractSeconds);
            }
        }

        if (destroyVfxPrefab != null)
            Instantiate(destroyVfxPrefab, transform.position, Quaternion.identity);

        if (destroyDelay <= 0f) Destroy(gameObject);
        else Destroy(gameObject, destroyDelay);
    } 
}
