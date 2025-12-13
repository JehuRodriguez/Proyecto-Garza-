using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputRaycaster : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);

            if (hit.collider)
            {
                Animal2 a = hit.collider.GetComponent<Animal2>();
                if (a) a.Hit();
            }
        }
    }
}
