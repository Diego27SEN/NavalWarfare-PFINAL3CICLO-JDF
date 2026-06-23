using System.Collections;
using TMPro;
using UnityEngine;

public class FadeOutText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textToFade;
    [SerializeField] private float fadeDuration = 5f;

    private void Start()
    {
        StartFade();
    }

    public void StartFade()
    {
        Debug.Log("Desvaneciendo texto...");
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float elapsedTime = 0f;

        Vector4 InitialColor = textToFade.color;
        InitialColor.w = 1f;

        Vector4 FinalColor = new Vector4(InitialColor.x, InitialColor.y, InitialColor.z, 0f);
        textToFade.color = InitialColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            Vector4 CurrentColor = Vector4.Lerp(InitialColor, FinalColor, elapsedTime / fadeDuration);
            textToFade.color = CurrentColor;

            yield return null;
        }
        textToFade.color = FinalColor;

        textToFade.gameObject.SetActive(false);
    }
}

/*Color originalColor = textToFade.color;
while (elapsedTime < fadeDuration)
{
    elapsedTime += Time.deltaTime;
    float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
    textToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    yield return null;
}
// Ensure the text is fully transparent at the end
textToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);*/