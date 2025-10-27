using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapTarget : MonoBehaviour
{
    [Header("Tipo")]
    public bool isEnemy = true;     
    public int scoreValue = 1;      
    [Header("Efectos")]
    public GameObject hitEffectPrefab; 
    public AudioClip hitSfx;

    public void OnTappedByPlayer()
    {
        if (GameManager.Instance == null) return;

        if (isEnemy)
        {
            GameManager.Instance.OnEnemyTapped(scoreValue);
        }
        else
        {
            GameManager.Instance.OnAllyTapped();
        }

        var ec = GetComponent<EnemyController>();

        if (ec != null)
        {
            ec.OnTapped(isEnemy, hitEffectPrefab, hitSfx);
        }

        else
        {
            if (hitEffectPrefab != null) Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
