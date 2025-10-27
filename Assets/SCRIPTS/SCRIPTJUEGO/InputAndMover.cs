using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Transform))]
public class InputAndMover : MonoBehaviour
{
    [Header("Movimiento")]
    public bool instantMove = true;    
    public float moveSpeed = 8f;

    Camera cam;
    Rigidbody2D rb;
    bool hasRigidbodyKinematic = false;

    void Awake()
    {
        cam = Camera.main;
        if (cam == null) Debug.LogError("[InputAndMover_Fix] ERROR: No MainCamera encontrada. Asegura tag MainCamera en la cámara.");
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.bodyType == RigidbodyType2D.Kinematic) hasRigidbodyKinematic = true;
            else
            {
                Debug.LogWarning("[InputAndMover_Fix] Rigidbody2D encontrado y NO es Kinematic. Esto puede sobrescribir transform.position. Recomiendo quitarlo o cambiar a Kinematic.");
            }
        }
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
                    Vector3 world = ScreenToWorldWithCorrectZ(t.position);
                    DoMove(world);
                    Debug.Log($"[InputAndMover_Fix] Touch detected. screen {t.position} -> world {world}");
                }
                else Debug.Log("[InputAndMover_Fix] Touch was over UI, ignored.");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!IsPointerOverUI())
            {
                Vector3 world = ScreenToWorldWithCorrectZ(Input.mousePosition);
                DoMove(world);
                Debug.Log($"[InputAndMover_Fix] Mouse click detected. screen {Input.mousePosition} -> world {world}");
            }
            else Debug.Log("[InputAndMover_Fix] Mouse click was over UI, ignored.");
        }
    }

    Vector3 ScreenToWorldWithCorrectZ(Vector2 screenPos)
    {
        
        float zDistance = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 sp = new Vector3(screenPos.x, screenPos.y, zDistance);
        Vector3 world = cam.ScreenToWorldPoint(sp);
        world.z = transform.position.z; 
        return world;
    }

    void DoMove(Vector3 world)
    {
        if (rb != null && hasRigidbodyKinematic)
        {
            if (instantMove)
            {
                rb.MovePosition(world);
                Debug.Log("[InputAndMover_Fix] rb.MovePosition (instant) -> " + world);
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(MoveSmoothRigidbody(world));
            }
            return;
        }

        if (instantMove)
        {
            transform.position = world;
            Debug.Log("[InputAndMover_Fix] transform.position (instant) -> " + world);
        }

        else
        {
            StopAllCoroutines();
            StartCoroutine(MoveSmoothTransform(world));
        }
    }

    System.Collections.IEnumerator MoveSmoothTransform(Vector3 target)
    {
        while ((Vector2)transform.position != (Vector2)target)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    System.Collections.IEnumerator MoveSmoothRigidbody(Vector3 target)
    {
        while ((Vector2)rb.position != (Vector2)target)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, (Vector2)target, moveSpeed * Time.deltaTime));
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
