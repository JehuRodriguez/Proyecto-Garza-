using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    public enum MovementMode { HorizontalOnly, FullMove, ReachOnly }
    public MovementMode movementMode = MovementMode.HorizontalOnly;


    public float speed = 8f;              
    public float attackRange = 0.4f;     
    public float reachVerticalTolerance = 1.2f; 
    public float returnSpeed = 8f;        
    public float baseY = -3.5f;
    [Tooltip("Tiempo de suavizado (segundos). Menor = más responsivo, mayor = más suave")]
    public float smoothTime = 0.06f;

    Rigidbody2D rb;
    Vector2 destination;
    Transform target;
    Vector2 velocityRef = Vector2.zero;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        destination = rb.position;
        Vector2 p = rb.position; p.y = baseY; rb.position = p;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            Collider2D hitCol = Physics2D.OverlapPoint(mouseWorld);
            if (hitCol != null)
            {
                Tap tapComp = hitCol.GetComponent<Tap>();
                if (tapComp != null) target = tapComp.transform;
                else target = null;
            }

            else target = null;

            if (movementMode == MovementMode.HorizontalOnly)
                destination = new Vector2(mouseWorld.x, baseY);
            else
                destination = new Vector2(mouseWorld.x, mouseWorld.y);
        }
    }

    void FixedUpdate()
    {
        Vector2 pos = rb.position;
        Vector2 desired = destination;

        if (movementMode == MovementMode.HorizontalOnly)
            desired.y = baseY;
        else if (movementMode == MovementMode.ReachOnly)
            desired.y = baseY;
        else if (movementMode == MovementMode.FullMove)
            if (target != null) desired = target.position;

        Vector2 newPos = Vector2.SmoothDamp(pos, desired, ref velocityRef, smoothTime, Mathf.Infinity, Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (movementMode == MovementMode.FullMove && target == null)
        {
            if (Mathf.Abs(rb.position.y - baseY) > 0.02f)
            {
                Vector2 backPos = new Vector2(rb.position.x, baseY);
                Vector2 n = Vector2.SmoothDamp(rb.position, backPos, ref velocityRef, 0.08f, Mathf.Infinity, Time.fixedDeltaTime);
                rb.MovePosition(n);
            }
        }

        if (target != null)
        {
            float dist = Vector2.Distance(rb.position, target.position);
            if (dist <= attackRange)
            {
                Tap tt = target.GetComponent<Tap>();
                if (tt != null) tt.OnTapped();
                target = null;
            }
        }

    }
}
