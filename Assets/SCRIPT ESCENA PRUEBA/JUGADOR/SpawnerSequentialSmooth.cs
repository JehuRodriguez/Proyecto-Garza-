using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSequentialSmooth : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject allyPrefab;

    public float spawnIntervalBetweenPairs = 1.8f;
    public float spawnDelayBetweenEnemyAndAlly = 0.45f;
    public float spawnXMin = -2.6f;
    public float spawnXMax = 2.6f;
    public float spawnY = 6f;
    public float entranceDrop = 0.8f; 
    public float entranceDuration = 0.4f; 
    public float minHorizontalSeparation = 0.9f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float baseX = Random.Range(spawnXMin, spawnXMax);

            
            Vector2 enemyStart = new Vector2(baseX, spawnY + entranceDrop);
            GameObject enemy = Instantiate(enemyPrefab, enemyStart, Quaternion.identity);
            
            StartCoroutine(EntranceMove(enemy.transform, new Vector2(baseX, spawnY), entranceDuration));

            
            yield return new WaitForSeconds(spawnDelayBetweenEnemyAndAlly);

            float candidateX = baseX + ((Random.value > 0.5f) ? minHorizontalSeparation : -minHorizontalSeparation);
            candidateX = Mathf.Clamp(candidateX, spawnXMin, spawnXMax);

            if (Mathf.Abs(candidateX - baseX) < minHorizontalSeparation - 0.1f)
            {
                candidateX = baseX + Mathf.Sign(candidateX - baseX) * (minHorizontalSeparation + 0.15f);
                candidateX = Mathf.Clamp(candidateX, spawnXMin, spawnXMax);
            }

            Vector2 allyStart = new Vector2(candidateX, spawnY + entranceDrop);
            GameObject ally = Instantiate(allyPrefab, allyStart, Quaternion.identity);
            StartCoroutine(EntranceMove(ally.transform, new Vector2(candidateX, spawnY), entranceDuration));

            yield return new WaitForSeconds(spawnIntervalBetweenPairs);
        }
    }

    IEnumerator EntranceMove(Transform t, Vector2 targetPos, float time)
    {
        if (t == null) yield break;
        Vector2 start = t.position;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float p = elapsed / time;
       
            float eased = Mathf.SmoothStep(0f, 1f, p);
            Vector2 newPos = Vector2.Lerp(start, targetPos, eased);
            t.position = newPos;
            yield return null;
        }
        t.position = targetPos;
    }
}
