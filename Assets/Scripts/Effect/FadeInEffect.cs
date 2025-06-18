using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class FadeInEffect : MonoBehaviour
{
    [SerializeField] private RectTransform container;
    [SerializeField] private TextMeshProUGUI wordPrefab;
    [SerializeField] private string paragraphText;
    [SerializeField] private float delayBetweenWords = 0.1f;
    [SerializeField] private float fadeDuration = 0.3f;

    private void Start()
    {
        StartCoroutine(FadeInWords());
    }

    IEnumerator FadeInWords()
    {
        string[] words = paragraphText.Split(' ');
        List<TextMeshProUGUI> wordObjects = new();

        foreach (var word in words)
        {
            var wordGO = Instantiate(wordPrefab, container);
            wordGO.text = word + " ";
            wordGO.alpha = 0;
            wordObjects.Add(wordGO);
        }

        foreach (var wordTMP in wordObjects)
        {
            yield return StartCoroutine(FadeInWord(wordTMP));
            yield return new WaitForSeconds(delayBetweenWords);
        }
    }

    IEnumerator FadeInWord(TextMeshProUGUI word)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            word.alpha = alpha;
            yield return null;
        }

        word.alpha = 1f;
    }
}
