
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSpawner : MonoBehaviour
{
   
    public Hole[] holes;
    public GameObject[] animals;
    public float spawnDelay = 1f;

    bool running;

    public void StartSpawning()
    {
        running = true;
        InvokeRepeating(nameof(Spawn), 0.5f, spawnDelay);
    }

    public void StopSpawning()
    {
        running = false;
        CancelInvoke();
    }

    void Spawn()
    {
        Hole hole = holes[Random.Range(0, holes.Length)];

        if (hole.occupied) return; 

        GameObject animal = animals[Random.Range(0, animals.Length)];
        GameObject spawned = Instantiate(animal, hole.spawnPoint.position, Quaternion.identity);

        hole.occupied = true;

        spawned.GetComponent<Animal2>().SetHole(hole);
    }

}
