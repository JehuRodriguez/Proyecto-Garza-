using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Transform))]
public class AutoTapper : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 6f;
    public float arriveThreshold = 0.08f;
    public bool instantMove = false; 

    [Header("AutoTapper - configuración")]
    public float range = 2.0f;
    public float attackInterval = 0.25f;
    public LayerMask targetLayer;
    public string enemyTag = "Enemy";
    public int fallbackScoreValue = 1;

    float attackTimer = 0f;
    Vector2 targetPosition;
    bool hasTargetPosition = false;
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogWarning("[AutoTapper] No main camera found. Tag your camera 'MainCamera'.");
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                if (!IsPointerOverUI(t.fingerId))
                {
                    SetTargetPositionFromScreen(t.position);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI())
            {
                SetTargetPositionFromScreen(Input.mousePosition);
            }
        }

        if (hasTargetPosition)
        {
            if (instantMove)
            {
                transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
                hasTargetPosition = false;
            }

            else
            {
                Vector2 cur = transform.position;
                Vector2 next = Vector2.MoveTowards(cur, targetPosition, moveSpeed * Time.deltaTime);
                transform.position = new Vector3(next.x, next.y, transform.position.z);
                if (Vector2.Distance(next, targetPosition) <= arriveThreshold) hasTargetPosition = false;
            }

        }

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = attackInterval;
            DoAutoTap();
        }
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    bool IsPointerOverUI(int fingerId)
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(fingerId);
    }



    void SetTargetPositionFromScreen(Vector2 screenPos)
    {
        if (cam == null) cam = Camera.main;
        Vector2 world = cam.ScreenToWorldPoint(screenPos);
        targetPosition = world;
        hasTargetPosition = true;
    }

    void DoAutoTap()
    {
        Collider2D[] hits = (targetLayer.value != 0)
            ? Physics2D.OverlapCircleAll(transform.position, range, targetLayer)
            : Physics2D.OverlapCircleAll(transform.position, range);

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
            if (GameManager.Instance != null) GameManager.Instance.OnEnemyTapped(fallbackScoreValue);
            Destroy(chosen.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.35f);
        Gizmos.DrawSphere(transform.position, range);
        Gizmos.color = Color.cyan;
        if (hasTargetPosition) Gizmos.DrawWireSphere(targetPosition, 0.08f);
    }
}



