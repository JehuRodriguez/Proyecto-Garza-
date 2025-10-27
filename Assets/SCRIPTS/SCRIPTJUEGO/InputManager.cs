using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                DoTap(t.position);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            DoTap(Input.mousePosition);
        }

    }

    void DoTap(Vector2 screenPos)
    {
        if (cam == null) return;
        Vector2 worldPoint = cam.ScreenToWorldPoint(screenPos);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {
            var target = hit.collider.GetComponent<TapTarget>();
            if (target != null)
            {
                target.OnTappedByPlayer();
            }
        }
    }
     
}
