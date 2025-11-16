using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRayCaster : MonoBehaviour
{
    Camera cam;
    void Start() { cam = Camera.main; }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 wp = cam.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Input click at world pos: " + wp);
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("Raycast hit: " + hit.collider.name);
                Animal a = hit.collider.GetComponent<Animal>();
                if (a != null) a.OnTapped();
            }
            else Debug.Log("Raycast hit nothing");
        }

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                Vector2 wp = cam.ScreenToWorldPoint(t.position);
                RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero);
                if (hit.collider != null)
                {
                    Animal a = hit.collider.GetComponent<Animal>();
                    if (a != null) a.OnTapped();
                }
            }
        }


    }
}
