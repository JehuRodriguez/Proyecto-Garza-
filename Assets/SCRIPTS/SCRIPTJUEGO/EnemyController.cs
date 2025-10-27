using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class EnemyController : MonoBehaviour
{
    bool dying = false;

    public void OnTapped(bool wasEnemy, GameObject effectPrefab, AudioClip sfx)
    {
        if (dying) return;
        dying = true;

        if (effectPrefab != null) Instantiate(effectPrefab, transform.position, Quaternion.identity);
        if (sfx != null) AudioSource.PlayClipAtPoint(sfx, Camera.main.transform.position);

        StartCoroutine(PopAndDestroy());
    }

    IEnumerator PopAndDestroy()
    {
        float dur = 0.08f;
        Vector3 start = transform.localScale;
        Vector3 end = Vector3.zero;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, end, t / dur);
            yield return null;
        }
        Destroy(gameObject);
    }


}
