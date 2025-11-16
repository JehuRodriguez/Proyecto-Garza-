using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Transform))]
public class SlotController : MonoBehaviour
{
    public GameObject animalPrefab;
    Animal currentAnimal;

    public void PopRandomAnimal(AnimalType type, bool isGood, int scoreValue, float visibleTime)
    {
        if (currentAnimal != null) return; 
        GameObject go = Instantiate(animalPrefab, transform.position, Quaternion.identity, transform);
        currentAnimal = go.GetComponent<Animal>();
        currentAnimal.type = type;
        currentAnimal.isGood = isGood;
        currentAnimal.scoreValue = scoreValue;
        currentAnimal.visibleTime = visibleTime;
        currentAnimal.ShowTemporarily();

        Destroy(go, visibleTime + 0.6f);
        StartCoroutine(ClearAfter(go, visibleTime + 0.6f));
    }

    IEnumerator ClearAfter(GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
        if (currentAnimal != null && go == currentAnimal.gameObject) currentAnimal = null;
    }
}
