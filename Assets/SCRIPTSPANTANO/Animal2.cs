using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal2 : MonoBehaviour
{
    public bool isInvasive; 

    bool active = true;
    GameController2 gameController;

    Hole myHole;

    void Start()
    {
        gameController = FindObjectOfType<GameController2>();
    }

    public void SetHole(Hole hole)
    {
        myHole = hole;
    }

    public void Hit()
    {
        if (!active) return;
        active = false;

        if (isInvasive)
        {
            gameController.AddScore(1);   
        }
        else
        {
            gameController.AddScore(-1);  
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (myHole != null)
            myHole.occupied = false;
    }
}
