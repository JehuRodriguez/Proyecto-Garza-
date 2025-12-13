using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal2 : MonoBehaviour
{
    public bool isInvasive;

    bool active = true;

    public void Hit()
    {
        if (!active) return;
        active = false;

        if (isInvasive)
            GameController2.Instance.AddScore();
        else
            GameController2.Instance.LoseLife();

        Destroy(gameObject);
    }




}
