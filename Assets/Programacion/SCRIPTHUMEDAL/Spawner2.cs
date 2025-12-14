using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2: MonoBehaviour
{
    [Header("Slots (assign Slot_0, Slot_1, ... here)")]
    public Transform[] slots;

    [Header("Prefabs per AnimalType - order MUST match AnimalType enum")]
    public GameObject[] animalPrefabs; 

    [Header("Timing")]
    public float startInterval = 1.0f;
    public float endInterval = 0.35f;
    public float roundDuration = 20f;
    public float visibleTime = 0.4f;

    bool running = false;

    public void StartRound()
    {
        running = true;
        StartCoroutine(SpawnRoutine());
    }

    public void StopRound()
    {
        running = false;
        StopAllCoroutines();
    }


    IEnumerator SpawnRoutine()
    {
        float elapsed = 0f;
        while (running && elapsed < roundDuration)
        {
            float progress = Mathf.Clamp01(elapsed / roundDuration);
            float interval = Mathf.Lerp(startInterval, endInterval, progress);

            if (slots != null && slots.Length > 0)
            {
                Transform slot = slots[Random.Range(0, slots.Length)];
                SpawnRandomSpeciesAtSlot(slot);
            }

            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
        if (GameManager2.Instance != null) GameManager2.Instance.OnRoundFinished();
    }

    void SpawnRandomSpeciesAtSlot(Transform slot)
    {
        int typeIndex = Random.Range(0, animalPrefabs.Length);
        GameObject prefab = animalPrefabs[typeIndex];
        if (prefab == null || slot == null) return;

        GameObject go = Instantiate(prefab, slot.position, Quaternion.identity, slot);
        Animal a = go.GetComponent<Animal>();
        if (a != null)
        {
          
            a.visibleTime = visibleTime;
            a.ShowTemporarily();
        }

        Destroy(go, visibleTime + 0.8f);
    }
}
