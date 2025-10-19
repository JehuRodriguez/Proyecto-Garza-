using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnEntry { public GameObject prefab; public float weight = 1f; }

    public SpawnEntry[] spawnables;
    public Transform[] spawnPoints;
    public float spawnInterval = 1.2f;
    private bool running = true;

    [Header("Referencias de escena (opcional)")]
    public Transform targetPoint;
    public PlayerCamuflaje playerCamo;

    private void Start()
    {
        if (targetPoint == null)
        {
            GameObject t = GameObject.Find("TargetPoint");
            if (t != null) targetPoint = t.transform;
        }

        if (playerCamo == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null)
            {
                playerCamo = p.GetComponent<PlayerCamuflaje>();
            }
        }



        StartCoroutine(SpawnLoop());
    }


    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (running)
            {
                SpawnOne();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOne()
    {
        if (spawnables.Length == 0 || spawnPoints.Length == 0 || spawnPoints == null || spawnPoints.Length == 0) return;
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        float total = 0f;
        foreach (var e in spawnables) total += Mathf.Max(0f, e.weight);
        if (total <= 0f) return;

        float r = Random.Range(0f, total);
        float acc = 0;
        foreach (var e in spawnables)
        {
            acc += Mathf.Max(0f, e.weight);
            if (r <= acc)
            {
                GameObject instance = Instantiate(e.prefab, sp.position, Quaternion.identity, transform);

                Interactable interact = instance.GetComponent<Interactable>();
                if (interact != null)
                {
                    if (targetPoint != null) interact.targetPoint = targetPoint;
                    if (playerCamo != null) interact.playerCamo = playerCamo;
                }
                return;
            }
        }
    }

    public void StopSpawning()
    {
        running = false;
    }


}
