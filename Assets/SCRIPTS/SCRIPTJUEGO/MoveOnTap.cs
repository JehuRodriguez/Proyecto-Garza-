using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Transform))]
public class MoveOnTap : MonoBehaviour
{
    [Header("Movimiento")]
    public bool instantMove = true;
    public float moveSpeed = 8f;

    [Header("Tap")]
    public bool triggerTapOnClick = true;
    public LayerMask interactableLayer = Physics2D.DefaultRaycastLayers;

    Camera cam;
    Rigidbody2D rb;
    bool hasKinematicRigidbody = false;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogError("[MoveAndTap] No MainCamera (tagged MainCamera) encontrada.");
        rb = GetComponent<Rigidbody2D>();
        if (rb != null && rb.bodyType == RigidbodyType2D.Kinematic) hasKinematicRigidbody = true;
    }


    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began && !IsPointerOverUI(t.fingerId))
            {
                ProcessInputAtScreenPos(t.position);
            }
        }


        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            ProcessInputAtScreenPos(Input.mousePosition);
        }
    }

    void ProcessInputAtScreenPos(Vector2 screenPos)
    {
        float playerScreenZ = cam.WorldToScreenPoint(transform.position).z;
        Vector3 spWithZ = new Vector3(screenPos.x, screenPos.y, playerScreenZ);
        Vector3 worldPoint = cam.ScreenToWorldPoint(spWithZ);
        worldPoint.z = transform.position.z;

        if (instantMove)
        {
            if (hasKinematicRigidbody) rb.MovePosition(worldPoint);
            else transform.position = worldPoint;
            Debug.Log("[MoveAndTap] Moved instantly to " + worldPoint);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(MoveSmooth(worldPoint));
        }

        if (triggerTapOnClick)
        {
            Collider2D hit = Physics2D.OverlapPoint(worldPoint, interactableLayer);
            if (hit != null)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Debug.Log("[MoveAndTap] Click on enemy -> calling OnEnemyTapped()");
                    GameManager.Instance.OnEnemyTapped(1); 
                }
                else if (hit.CompareTag("Ally"))
                {
                    Debug.Log("[MoveAndTap] Click on ally -> calling OnAllyTapped()");
                    GameManager.Instance.OnAllyTapped();
                }
                else
                {
                    Debug.Log("[MoveAndTap] Click on collider but no valid tag found on " + hit.name);
                }
            }
        }
    }

    System.Collections.IEnumerator MoveSmooth(Vector3 target)
    {
        while ((Vector2)transform.position != (Vector2)target)
        {
            Vector2 next = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            if (hasKinematicRigidbody) rb.MovePosition(next);
            else transform.position = next;
            yield return null;
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
}
