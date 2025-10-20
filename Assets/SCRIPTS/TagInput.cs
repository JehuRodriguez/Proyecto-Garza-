using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TagInput : MonoBehaviour
{
    [Header("Referencias")]
    public Camera mainCamera;
    public PlayerCamuflaje playerCamo;
    public PlayerMover playerMover;

    [Header("Ajustes")]
    public float tapRadius = 0.6f;

     void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchSupported && Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began) HandleTap(t.position);
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
        Vector3 wp3 = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, cameraZ));
        Vector2 wp = new Vector2(wp3.x, wp3.y);

        Collider2D[] hits = Physics2D.OverlapCircleAll(wp, tapRadius);
        if (hits == null || hits.Length == 0)
        {
            if (playerMover != null) playerMover.MoveTo(wp);
            return;
        }

        var interactables = hits
           .Select(h => new { col = h, dist = Vector2.Distance(h.ClosestPoint(wp), wp) })
           .Select(x => new { collider = x.col, dist = x.dist, interact = x.col.GetComponentInParent<Interactable>() })
           .Where(x => x.interact != null)
           .OrderBy(x => x.dist)
           .ToArray();

        if (interactables.Length > 0)
        {
            var chosen = interactables[0];
            Interactable interact = chosen.interact;
            Debug.Log($"TapInput: seleccionado {interact.name} (tipo={interact.entityType}) a distancia {chosen.dist}");
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

        if (playerMover != null) playerMover.MoveTo(wp);

    }

    

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (mainCamera == null) mainCamera = Camera.main;
        if (mainCamera == null) return;
        Vector3 mousePos = Input.mousePosition;
        float cameraZ = -mainCamera.transform.position.z;
        Vector3 wp = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cameraZ));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(wp.x, wp.y, 0f), tapRadius);
#endif
    }



}
