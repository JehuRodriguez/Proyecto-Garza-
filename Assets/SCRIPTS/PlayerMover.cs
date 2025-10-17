using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    public float moveSpeed = 4f;
    public float stoppingDistance = 0.15f;

    Rigidbody2D rb;
    Vector2 targetPos;
    Interactable targetInteractable = null;
    public bool isMoving { get; private set; } = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        targetPos = rb.position;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        Vector2 current = rb.position;
        Vector2 dir = targetPos - current;
        float dist = dir.magnitude;
        if (dist <= stoppingDistance)
        {
            rb.MovePosition(targetPos);
            isMoving = false;
            OnArrived();
            return;
        }

        Vector2 step = dir.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(current + step);
    }

    public void MoveTo(Vector2 worldPos)
    {
        targetInteractable = null;
        targetPos = worldPos;
        isMoving = true;
    }

    public void MoveToInteractable(Interactable interact)
    {
        if (interact == null)
        {
            return;
        }

        targetInteractable = interact;
        targetPos = interact.transform.position;
        isMoving = true;
    }

    public void CancelMove()
    {
        targetInteractable = null;
        isMoving = false;
    }

    void OnArrived()
    {
        if (targetInteractable != null)
        {
            if (targetInteractable != null)
            {
                bool duringCamouflage = false;
                targetInteractable.OnTappedAtArrival();
            }
        }

        targetInteractable = null;
    }



}
