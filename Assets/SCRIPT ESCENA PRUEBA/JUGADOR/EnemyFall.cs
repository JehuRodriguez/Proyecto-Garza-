using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFall : MonoBehaviour
{
    public float fallSpeed = 2f;         
    public float destroyBelowY = -6f;

    void Update()
    {
        
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        
        if (transform.position.y < destroyBelowY)
        {
            Destroy(gameObject);
        }
    }
}
