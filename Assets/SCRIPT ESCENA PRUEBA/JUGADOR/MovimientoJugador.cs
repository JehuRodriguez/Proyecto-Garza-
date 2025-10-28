using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float smoothTime = 0.3f; 

    private Vector3 velocity = Vector3.zero;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; 
            StartCoroutine(MoveToPosition(mousePos));
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f) 
        {
            rb.MovePosition(Vector3.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime));
            yield return null;
        }
    }
}
