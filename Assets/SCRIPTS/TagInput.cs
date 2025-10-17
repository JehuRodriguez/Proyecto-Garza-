using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagInput : MonoBehaviour
{
    [Header("Referencias")]
    public Camera mainCamera;
    public PlayerCamuflaje playerCamo;
    public PlayerMover playerMover;

    [Header("Ajustes")]
    public float tapRadius = 0.6f;

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchSupported && Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                HandleTap(t.position);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            HandleTap(Input.mousePosition);
        }
    }

    void HandleTap(Vector2 screenPos)
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

        float cameraZ = -mainCamera.transform.position.z;
        Vector3 wp = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, cameraZ));
        Vector2 worldPos2D = new Vector2(wp.x, wp.y);

        Collider2D[] hits = Physics2D.OverlapCircleAll(worldPos2D, tapRadius);
        foreach (var c in hits)
        {
            Interactable interact = c.GetComponent<Interactable>();
            if (interact != null)
            {
                if (playerMover != null)
                {

                    playerMover.MoveToInteractable(interact);
                }

                else
                {
                    
                    bool camo = (playerCamo != null && playerCamo.IsCamouflaged);
                    interact.OnTapped(camo);
                }

                return;
            }
        }
        if (playerMover != null)
        {
            playerMover.MoveTo(worldPos2D);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;

#if UNITY_EDITOR
        Vector3 mousePos = Input.mousePosition;
        float cameraZ = -mainCamera.transform.position.z;
        Vector3 wp = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraZ));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(wp.x, wp.y, 0f), tapRadius);
#endif
    }



}
