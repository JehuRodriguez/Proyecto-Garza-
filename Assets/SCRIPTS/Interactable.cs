using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType { Enemy, Ally }

public class Interactable : MonoBehaviour
{
    [Header("Tipo de entidad")]
    public EntityType entityType = EntityType.Enemy;


    [Header("Configuración de la entidad")]
    public bool isEnemy = true;
    public float damageOnReach = -10f;
    public float rewardOnTap = 5f;
    public float speed = 0.5f;

    [Header("Referencias")]
    public Transform targetPoint;
    public PlayerCamuflaje playerCamo;

    [Header("Valores para la barra de jugador")]
    public float playerGainOnEnemy = 10f;
    public float playerLossOnAlly = 15f;

    private bool reached = false;

    private void Update()
    {
        if (targetPoint == null) return;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
        if (!reached && Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            reached = true;
            OnReachedTarget();
        }
    }

    void OnReachedTarget()
    {
        if (entityType == EntityType.Enemy)
        {
            if (GameManager.Instance != null) GameManager.Instance.ChangeHealth(damageOnReach);
            Debug.Log($"{name} llegó al target como ENEMY -> daño {damageOnReach}");
            Destroy(gameObject);
        }
        else
        {
            if (GameManager.Instance != null) GameManager.Instance.ChangeHealth(rewardOnTap);
            Debug.Log($"{name} llegó al target como ALLY -> reward {rewardOnTap}");
            Destroy(gameObject);
        }
    }

    public void HandleTapFromInput()
    {
        bool camo = (playerCamo != null && playerCamo.IsCamouflaged);
        OnTapped(camo);
    }

    public void OnTapped(bool duringCamouflage)
    {
        if (entityType == EntityType.Enemy)
        {
            float reward = rewardOnTap * (duringCamouflage ? 2f : 1f);
            if (GameManager.Instance != null) GameManager.Instance.ChangeHealth(reward);
            Debug.Log($"{name} OnTapped ENEMY, camo={duringCamouflage}, reward={reward}");

            if (PlayerLifeUI.Instance != null)
                PlayerLifeUI.Instance.AddLife(playerGainOnEnemy);
            Destroy(gameObject);
        }
        else
        {
            if (GameManager.Instance != null) GameManager.Instance.ChangeHealth(-10f);
            Debug.Log($"{name} OnTapped ALLY -> penaliza al ecosistema");

            if (PlayerLifeUI.Instance != null) PlayerLifeUI.Instance.RemoveLife(playerLossOnAlly);
            Destroy(gameObject);
        }
    }

    public void OnTappedAtArrival()
    {

        if (entityType == EntityType.Enemy)
        {
            if (GameManager.Instance != null) GameManager.Instance.ChangeHealth(rewardOnTap);
            Debug.Log($"{name} OnTappedAtArrival ENEMY -> reward {rewardOnTap}");

            if (PlayerLifeUI.Instance != null) PlayerLifeUI.Instance.AddLife(playerGainOnEnemy);
            Destroy(gameObject);
        }
        else
        {
            if (GameManager.Instance != null) GameManager.Instance.ChangeHealth(-10f);
            Debug.Log($"{name} OnTappedAtArrival ALLY -> penaliza");
            if (PlayerLifeUI.Instance != null) PlayerLifeUI.Instance.RemoveLife(playerLossOnAlly);
            Destroy(gameObject); 
        }
    }


}
