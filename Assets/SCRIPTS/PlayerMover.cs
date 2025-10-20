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
    public Interactable targetInteractable = null;
    public PlayerCamuflaje playerCamo;
    public bool isMoving { get; private set; } = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        targetPos = rb.position;
    }

    void Start()
    {
        if (playerCamo == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) playerCamo = p.GetComponent<PlayerCamuflaje>();
        }
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        if (targetInteractable != null)
        {
            
            if (targetInteractable.gameObject == null)
            {
                CancelMove();
                return;
            }
            targetPos = targetInteractable.transform.position;
        }

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
        Debug.Log("PlayerMover: MoveTo " + targetPos);
    }

    public void MoveToInteractable(Interactable interact)
    {
        if (interact == null) return;
        targetInteractable = interact;
        targetPos = interact.transform.position; 
        isMoving = true;
        Debug.Log("PlayerMover: MoveToInteractable -> " + interact.name);
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
            bool camo = (playerCamo != null && playerCamo.IsCamouflaged);
            Debug.Log($"PlayerMover: llegó a {targetInteractable.name} (camo={camo}) — llamando OnTapped");
            targetInteractable.OnTapped(camo);

        }
        targetInteractable = null;
    }



}
