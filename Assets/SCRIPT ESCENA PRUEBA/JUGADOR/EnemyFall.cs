using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFall : MonoBehaviour
{
    [Header("Velocidad")]
    public float baseFallSpeed = 2.5f;            
    public float speedIncreasePerMinute = 2.5f; 
    public float maxFallSpeed = 8f;               

    public float destroyBelowY = -6f;

    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (TimeManager.GameIsOver || (TimeManager.Instance != null && !TimeManager.Instance.IsRunning))
            return;

        float minutes = Time.timeSinceLevelLoad / 60f;
        float fallSpeed = baseFallSpeed + minutes * speedIncreasePerMinute;
        fallSpeed = Mathf.Min(fallSpeed, maxFallSpeed);

        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < destroyBelowY)
        {
            Destroy(gameObject);
        }
    }
}
