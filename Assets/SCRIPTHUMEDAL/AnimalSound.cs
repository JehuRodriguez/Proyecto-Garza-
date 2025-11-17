using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSound : MonoBehaviour
{
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if (audioSource != null)
            audioSource.Play();
    }
}
