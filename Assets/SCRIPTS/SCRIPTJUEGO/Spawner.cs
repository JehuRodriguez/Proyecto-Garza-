using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject allyPrefab;

    [Header("Spawn settings")]
    public float spawnInterval = 0.6f;
    public float spawnVariance = 0.4f;
    public float xRange = 2.5f;   
    public float spawnY = 6f;     
    public float minSpeed = 1.5f;
    public float maxSpeed = 4f;
    public float enemyProbability = 0.75f;

    bool running = true;

    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (running)
        {
            SpawnOne();
            float wait = spawnInterval + Random.Range(-spawnVariance, spawnVariance);
            if (wait < 0.05f) wait = 0.05f;
            yield return new WaitForSeconds(wait);
        }
    }

    void SpawnOne()
    {
        GameObject prefab = (Random.value < enemyProbability) ? enemyPrefab : allyPrefab;
        Vector3 worldPos = new Vector3(transform.position.x + Random.Range(-xRange, xRange), spawnY, 0f);
        GameObject obj = Instantiate(prefab, worldPos, Quaternion.identity);
        float speed = Random.Range(minSpeed, maxSpeed);
        var mover = obj.GetComponent<FallMovement>();
        if (mover == null) mover = obj.AddComponent<FallMovement>();
        mover.speed = speed;
    }

    public void StopSpawner()
    {
        running = false;
        StopAllCoroutines();
    }

}
