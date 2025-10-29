using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tap : MonoBehaviour
{
    public bool isEnemy = true;               
    public GameObject destroyVfxPrefab;       
    public float destroyDelay = 0f;

    public void OnTapped()
    {
        
        if (destroyVfxPrefab != null)
        {
            Instantiate(destroyVfxPrefab, transform.position, Quaternion.identity);
        }

        if (destroyDelay <= 0f)
            Destroy(gameObject);
        else
            Destroy(gameObject, destroyDelay);
    }

   
}
