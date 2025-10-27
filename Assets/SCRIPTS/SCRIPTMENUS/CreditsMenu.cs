using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsMenu : MonoBehaviour
{
  
    [Header("Nombres del equipo")]
    public TextMeshProUGUI[] memberTexts;

    [Header("Botón de regreso")]
    public Button backButton;
    [Tooltip("Nombre exacto de la escena del Menú Principal")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Animación de aparición")]
    public float fadeDuration = 2f;
    public float moveDistance = 50f; 

    private float timer = 0f;
    private bool animating = true;

    private Vector3[] startPositions;
    private Color[] startColors;

    void Start()
    {
        if (memberTexts == null || memberTexts.Length == 0)
        {
            Debug.LogWarning("No se asignaron textos de miembros en el inspector.");
            return;
        }

        startPositions = new Vector3[memberTexts.Length];
        startColors = new Color[memberTexts.Length];

        for (int i = 0; i < memberTexts.Length; i++)
        {
            startPositions[i] = memberTexts[i].rectTransform.anchoredPosition - new Vector2(0, moveDistance);
            memberTexts[i].rectTransform.anchoredPosition = startPositions[i];
            startColors[i] = memberTexts[i].color;
            memberTexts[i].color = new Color(startColors[i].r, startColors[i].g, startColors[i].b, 0);
        }

        if (backButton != null)
            backButton.onClick.AddListener(ReturnToMainMenu);
    }

    void Update()
    {
        if (!animating) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / fadeDuration);

        for (int i = 0; i < memberTexts.Length; i++)
        {
            Color c = memberTexts[i].color;
            c.a = Mathf.Lerp(0, 1, t);
            memberTexts[i].color = c;

            Vector3 pos = Vector3.Lerp(startPositions[i], startPositions[i] + new Vector3(0, moveDistance, 0), t);
            memberTexts[i].rectTransform.anchoredPosition = pos;
        }

        if (t >= 1f)
            animating = false;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }


}
