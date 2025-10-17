using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamuflaje : MonoBehaviour
{
    public float camoDuration = 1.8f;
    public float camoCooldown = 5f;
    public bool IsCamouflaged { get; private set; } = false;
    private bool onCooldown = false;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) originalColor = spriteRenderer.color;
    }

    public void TryActivate()
    {
        if (onCooldown) return;
        StartCoroutine(CamoRoutine());
    }

    IEnumerator CamoRoutine()
    {
        IsCamouflaged = true;
        onCooldown = true;
        if (spriteRenderer) spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.6f);
        yield return new WaitForSeconds(camoDuration);
        IsCamouflaged = false;
        if (spriteRenderer) spriteRenderer.color = originalColor;
        yield return new WaitForSeconds(camoCooldown);
        onCooldown = false;
    }

    public float GetCooldownNormalized()
    {
        return onCooldown ? 1f : 0f;
    }

}
