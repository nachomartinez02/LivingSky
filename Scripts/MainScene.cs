using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public GameObject panelMainScene;
    public GameObject panelCharacters;
    public GameObject panelLoading;

    public float transitionDuration = 3f; // Duración de la transición en segundos
    private bool isTransitioning = false;

    public TextMeshProUGUI loadingText;

    private CanvasGroup panelCanvasGroup;
    void Start()
    {
        panelCanvasGroup = panelMainScene.GetComponent<CanvasGroup>();
        panelCharacters.SetActive(false);
        panelMainScene.SetActive(true);
    }

    public void Play()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionPanel());
        }
    }
    IEnumerator TransitionPanel()
    {
        isTransitioning = true;

        // Obtener el componente CanvasGroup
        if (panelCanvasGroup == null)
        {
            Debug.LogError("El objeto 'panelMainScene' no tiene un componente CanvasGroup adjunto.");
            yield break; // Salir de la corrutina si no hay CanvasGroup
        }

        float elapsedTime = 0f;
        float startAlpha = panelCanvasGroup.alpha;
        float endAlpha = 0f; // Transparente

        while (elapsedTime < transitionDuration)
        {
            panelCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / transitionDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de establecer el alpha final explícitamente para evitar posibles errores de interpolación.
        panelCanvasGroup.alpha = endAlpha;

        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;

        panelMainScene.SetActive(false);

        publicVariables.createScene = 1;
        panelLoading.SetActive(true);
        StartCoroutine(LoadingAnimation());

        isTransitioning = false;
    }

    IEnumerator LoadingAnimation()
    {
        while (publicVariables.createScene == 1 || publicVariables.createScene == 2)
        {
            loadingText.text = "Loading terrain . ";
            yield return new WaitForSeconds(0.5f); // Tiempo de espera entre cada punto

            loadingText.text = "Loading terrain . . ";
            yield return new WaitForSeconds(0.5f);

            loadingText.text = "Loading terrain . . . ";
            yield return new WaitForSeconds(0.5f);

            // Puedes agregar más puntos según sea necesario

            yield return null; // Esperar un frame antes de repetir el bucle
        }

        // Cuando movements es igual a 1, puedes realizar alguna acción adicional aquí.
        loadingText.text = "Loading terraing completed ! ! :)";
        yield return new WaitForSeconds(1f);
        panelLoading.SetActive(false);
        panelCharacters.SetActive(true);

    }
}
