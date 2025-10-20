using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EntityType
{
    Enemy,
    Ally,
    Neutral
}

public class Interactable : MonoBehaviour
{
    [Header("Tipo de entidad")]
    public EntityType entityType = EntityType.Enemy;

    [Header("Movimiento y llegada")]
    public float moveSpeed = 1.2f;         
    public float interactionRange = 1.2f;  
    public float reachThreshold = 0.12f;

    [Header("Efectos al llegar al objetivo (opcional)")]
    public float damageOnReach = -10f;     
    public float rewardOnReach = 5f;

    [HideInInspector] public Transform targetPoint;
    [HideInInspector] public PlayerCamuflaje playerCamo;

    [Header("Efecto al ser tocado por el jugador")]
    public float playerLifeChange = 10f;   

     bool interacted = false;
    bool reachedTarget = false;
    Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        else player = null;
    }

    void Update()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (interacted) return;

        if (targetPoint != null && !reachedTarget)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);
            transform.position = newPos;

            float distToTarget = Vector2.Distance(transform.position, targetPoint.position);
            if (distToTarget <= reachThreshold)
            {
                reachedTarget = true;
                OnReachedTarget();
                return;
            }
        }

        if (player != null)
        {
            float distToPlayer = Vector2.Distance(player.position, transform.position);
            if (!interacted && distToPlayer <= interactionRange)
            {
                interacted = true;
                bool camo = (playerCamo != null && playerCamo.IsCamouflaged);
                OnTapped(camo); 
            }
        }
    }

    public void OnTapped(bool isCamouflaged = false)
    {
        float multiplier = (entityType == EntityType.Enemy && isCamouflaged) ? 2f : 1f;

        if (entityType == EntityType.Enemy)
        {
            float add = playerLifeChange * multiplier;
            PlayerLifeUI.Instance?.AddLife(add);
            Debug.Log($"{name}: interceptado ENEMY -> +{add} vida al jugador");
        }

        else if (entityType == EntityType.Ally)
        {
            PlayerLifeUI.Instance?.RemoveLife(playerLifeChange);
            Debug.Log($"{name}: interceptado ALLY -> -{playerLifeChange} vida al jugador");
        }
        else
        {
            Debug.Log($"{name}: interceptado NEUTRAL (sin efecto de vida)");
        }

        Destroy(gameObject);

    }

    void OnReachedTarget()
    {
       
        if (GameManager.Instance != null)
        {
            if (entityType == EntityType.Enemy)
            {
                GameManager.Instance.ChangeHealth(damageOnReach);
                Debug.Log($"{name} llegó al refugio: ENEMY dañó ecosistema {damageOnReach}");
            }
            else if (entityType == EntityType.Ally)
            {
                GameManager.Instance.ChangeHealth(rewardOnReach);
                Debug.Log($"{name} llegó al refugio: ALLY benefició ecosistema {rewardOnReach}");
            }
        }
        Destroy(gameObject);
    }
}
