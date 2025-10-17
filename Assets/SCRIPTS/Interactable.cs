using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Configuración de la entidad")]
    public bool isEnemy = true;
    public float damageOnReach = -10f;
    public float rewardOnTap = 5f;
    public float speed = 0.5f;

    [Header("Referencias")]
    public Transform targetPoint;
    public PlayerCamuflaje playerCamo;

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
        if (isEnemy)
        {
            GameManager.Instance.ChangeHealth(damageOnReach);
            Destroy(gameObject);
        }
        else
        {
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
        if (isEnemy)
        {
            float reward = rewardOnTap * (duringCamouflage ? 2f : 1f);
            GameManager.Instance.ChangeHealth(reward);
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.ChangeHealth(-10f);
            Destroy(gameObject);
        }
    }

    public void OnTappedAtArrival()
    {

        if (isEnemy)
        {
            float reward = rewardOnTap;
            GameManager.Instance.ChangeHealth(reward);
            Destroy(gameObject);
        }
        else
        {
            GameManager.Instance.ChangeHealth(-10f);
            Destroy(gameObject);
        }
    }


}
