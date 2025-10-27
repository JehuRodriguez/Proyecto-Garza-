using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Transform))]
public class AutoTapper : MonoBehaviour
{
    [Header("AutoTapper - configuración")]
    public float range = 2.0f;                 
    public float attackInterval = 0.25f;       
    public LayerMask targetLayer;              
    public string enemyTag = "Enemy";          
    public int fallbackScoreValue = 1;         

    float attackTimer = 0f;

    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = attackInterval;
            DoAutoTap();
        }
    }

    void DoAutoTap()
    {
        Collider2D[] hits;
        if (targetLayer.value != 0)
            hits = Physics2D.OverlapCircleAll(transform.position, range, targetLayer);
        else
            hits = Physics2D.OverlapCircleAll(transform.position, range);

        if (hits == null || hits.Length == 0) return;

        Collider2D chosen = hits
            .Where(h => h != null)
            .OrderBy(h => Vector2.Distance(transform.position, h.transform.position))
            .FirstOrDefault(h => h.CompareTag(enemyTag));

        if (chosen == null)
        {
            chosen = hits
                .Where(h => h != null && h.GetComponent<TapTarget>() != null && h.GetComponent<TapTarget>().isEnemy)
                .OrderBy(h => Vector2.Distance(transform.position, h.transform.position))
                .FirstOrDefault();
        }

        if (chosen == null) return;

        
        var target = chosen.GetComponent<TapTarget>();
        if (target != null)
        {
            target.OnTappedByPlayer();
            return;
        }

        if (chosen.CompareTag(enemyTag))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnEnemyTapped(fallbackScoreValue);
            }
            Destroy(chosen.gameObject);
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.35f);
        Gizmos.DrawSphere(transform.position, range);
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
