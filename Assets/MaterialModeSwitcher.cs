
using UnityEngine;
using TMPro;
using System.Collections;

public class MaterialModeSwitcher : MonoBehaviour
{
    public Renderer targetRenderer;
    public Material currentMaterial;
    public Material alternativeMaterial;
    public TMP_Text buttonText;

    public float transitionDuration = 1f;

    private bool alternativeMode = false;
    private bool isTransitioning = false;
    private Material runtimeMaterial;

    void Start()
    {
        runtimeMaterial = new Material(currentMaterial);
        targetRenderer.material = runtimeMaterial;
        UpdateButtonText();
    }

    public void SwitchMode()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ChangeMode());
    }

    IEnumerator ChangeMode()
    {
        isTransitioning = true;

        Color startColor = runtimeMaterial.color;

        Color targetColor = alternativeMode
            ? currentMaterial.color
            : alternativeMaterial.color;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / transitionDuration;

            runtimeMaterial.color = Color.Lerp(
                startColor,
                targetColor,
                progress
            );

            yield return null;
        }

        runtimeMaterial.color = targetColor;

        alternativeMode = !alternativeMode;
        UpdateButtonText();

        isTransitioning = false;
    }

    void UpdateButtonText()
    {
        buttonText.text = alternativeMode
            ? "Günümüz Moduna Dön"
            : "Alternatif Moda Geç";
    }
}
