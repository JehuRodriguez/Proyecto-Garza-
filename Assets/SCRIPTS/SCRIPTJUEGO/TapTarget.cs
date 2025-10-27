using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TapTarget : MonoBehaviour
{
    public bool isEnemy = true;
    public int scoreValue = 1;
    bool handled = false;

    public void OnTappedByPlayer()
    {
        if (handled) return;
        handled = true;

        if (isEnemy || gameObject.CompareTag("Enemy"))
        {
            if (GameManager.Instance != null) GameManager.Instance.OnEnemyTapped(scoreValue);
        }

        else
        {
            if (GameManager.Instance != null) GameManager.Instance.OnAllyTapped();
        }

        Destroy(gameObject);
    }
}
