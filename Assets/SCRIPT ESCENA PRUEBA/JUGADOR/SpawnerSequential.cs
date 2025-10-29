using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSequential : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject allyPrefab;
    public float spawnIntervalBetweenPairs = 1.5f; 
    public float spawnDelayBetweenEnemyAndAlly = 0.45f; 
    public float spawnXMin = -2.5f;
    public float spawnXMax = 2.5f;
    public float spawnY = 5f; 
    public float horizontalOffsetWhenSpawning = 0.6f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float baseX = Random.Range(spawnXMin, spawnXMax);

            
            Vector2 enemyPos = new Vector2(baseX, spawnY);
            SpawnOne(enemyPrefab, enemyPos);

           
            yield return new WaitForSeconds(spawnDelayBetweenEnemyAndAlly);

            float allyX = baseX + ((Random.value > 0.5f) ? horizontalOffsetWhenSpawning : -horizontalOffsetWhenSpawning);

           
            allyX = Mathf.Clamp(allyX, spawnXMin, spawnXMax);

            Vector2 allyPos = new Vector2(allyX, spawnY);
            SpawnOne(allyPrefab, allyPos);


            yield return new WaitForSeconds(spawnIntervalBetweenPairs);
        }
    }

    void SpawnOne(GameObject prefab, Vector2 pos)
    {
        if (prefab == null) return;
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
    }
}
