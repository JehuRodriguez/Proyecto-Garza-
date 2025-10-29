using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerOneByOne : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject allyPrefab;

    [Header("Timing")]
    public float spawnIntervalBetweenSingles = 0.9f; 
    public float entranceDrop = 0.8f;
    public float entranceDuration = 0.35f;

    [Header("Spread (se calcula automáticamente si true)")]
    public bool autoCameraBounds = true;
    public float spawnXMin = -2.6f;
    public float spawnXMax = 2.6f;
    public float spawnY = 6f;

    [Header("Wave settings (opcional)")]
    public bool waveMode = true;
    public float timeBetweenWaves = 2.0f;
    public int maxWaveCount = 999;

    Camera mainCam;
    int waveIndex = 0;

    void Start()
    {
        mainCam = Camera.main;
        if (autoCameraBounds) CalculateBoundsFromCamera();
        StartCoroutine(SpawnLoop());
    }

    void CalculateBoundsFromCamera()
    {
        float camY = mainCam.transform.position.y;
        float camX = mainCam.transform.position.x;
        float halfWidth = mainCam.orthographicSize * mainCam.aspect;
        spawnXMin = camX - halfWidth + 0.5f; 
        spawnXMax = camX + halfWidth - 0.5f;
        spawnY = camY + mainCam.orthographicSize + 1f; 
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (!waveMode)
            {
                SpawnSingleRandom();
                yield return new WaitForSeconds(spawnIntervalBetweenSingles);
            }
            else
            {
                waveIndex++;

                int enemyCount = 1 + (waveIndex / 2);
                int allyCount = 1 + (waveIndex / 3);


                bool startWithEnemy = (Random.value > 0.35f);

                int spawnedEnemies = 0;
                int spawnedAllies = 0;
                while (spawnedEnemies < enemyCount || spawnedAllies < allyCount)
                {
                    if (startWithEnemy)
                    {
                        if (spawnedEnemies < enemyCount)
                        {
                            SpawnAtRandomX(enemyPrefab);
                            spawnedEnemies++;
                            yield return new WaitForSeconds(spawnIntervalBetweenSingles);
                        }

                        if (spawnedAllies < allyCount)
                        {
                            SpawnAtRandomX(allyPrefab);
                            spawnedAllies++;
                            yield return new WaitForSeconds(spawnIntervalBetweenSingles);
                        }
                    }
                    else
                    {
                        if (spawnedAllies < allyCount)
                        {
                            SpawnAtRandomX(allyPrefab);
                            spawnedAllies++;
                            yield return new WaitForSeconds(spawnIntervalBetweenSingles);
                        }
                        if (spawnedEnemies < enemyCount)
                        {
                            SpawnAtRandomX(enemyPrefab);
                            spawnedEnemies++;
                            yield return new WaitForSeconds(spawnIntervalBetweenSingles);
                        }

                    }
                }

                yield return new WaitForSeconds(timeBetweenWaves);

                if (waveIndex >= maxWaveCount) waveIndex = 0;
            }
        }
    }

    void SpawnSingleRandom()
    {
        
        float r = Random.value;
        if (r < 0.7f) SpawnAtRandomX(enemyPrefab);
        else SpawnAtRandomX(allyPrefab);
    }

    void SpawnAtRandomX(GameObject prefab)
    {
        if (prefab == null) return;
        float x = Random.Range(spawnXMin, spawnXMax);
        Vector2 start = new Vector2(x, spawnY + entranceDrop);
        GameObject go = Instantiate(prefab, start, Quaternion.identity);
        StartCoroutine(EntranceMove(go.transform, new Vector2(x, spawnY), entranceDuration));
    }

    IEnumerator EntranceMove(Transform t, Vector2 targetPos, float time)
    {
        if (t == null) yield break;
        Vector2 start = t.position;
        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float p = Mathf.Clamp01(elapsed / time);
            float eased = Mathf.SmoothStep(0f, 1f, p);
            Vector2 newPos = Vector2.Lerp(start, targetPos, eased);
            t.position = newPos;
            yield return null;
        }
        t.position = targetPos;
    }

}
