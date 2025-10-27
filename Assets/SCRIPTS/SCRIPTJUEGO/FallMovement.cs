using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallMovement : MonoBehaviour
{
    public float speed = 2.5f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        
        if (transform.position.y < -12f) 
            Destroy(gameObject);
    }
}
