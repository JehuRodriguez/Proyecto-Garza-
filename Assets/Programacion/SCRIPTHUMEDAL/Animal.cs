using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AnimalType { Garza, Garceta, Gallito, Zambullidor, Gaviota, Patito, Rata, Perro }

public class Animal : MonoBehaviour
{
    public AnimalType type;
    public bool isGood = false; 
    public int scoreValue = 10;
    public float visibleTime = 0.4f;

    SpriteRenderer sr;
    Collider2D col;
    bool activeVisible = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        if (col == null) gameObject.AddComponent<BoxCollider2D>(); 
        HideImmediate();
    }

    public void ShowTemporarily()
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        activeVisible = true;
       
        transform.localScale = Vector3.zero;
        float t = 0f;
        float dur = 0.12f;
        while (t < dur)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t / dur);
            yield return null;
        }
        transform.localScale = Vector3.one;

        yield return new WaitForSeconds(visibleTime);

        t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t / dur);
            yield return null;
        }

        HideImmediate();
    }

    void HideImmediate()
    {
        activeVisible = false;
        transform.localScale = Vector3.zero;
        
    }

    public void OnTapped()
    {
        if (!activeVisible) return;

        activeVisible = false;

        if (isGood)
        {
            GameManager2.Instance.AddScore(scoreValue);
          
        }
        else
        {
            GameManager2.Instance.LoseLife(1);
        }
        StartCoroutine(HitAndHide());
    }

    IEnumerator HitAndHide()
    {
        Vector3 start = transform.localScale;
        float t = 0f;
        float dur = 0.08f;
        while (t < dur)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, start * 0.6f, t / dur);
            yield return null;
        }

        transform.localScale = Vector3.zero;
    }
}
