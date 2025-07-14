using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFadeIn : MonoBehaviour
{
    private Material cubeMat;
    public float fadeDuration = 0.5f;

    private void Awake()
    {
        cubeMat = GetComponent<Renderer>().material;
    }

    public void StartFade()
    {
        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        Color color = cubeMat.color;
        color.a = 0f;
        cubeMat.color = color;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            cubeMat.color = color;
            yield return null;
        }

        color.a = 1f;
        cubeMat.color = color;
    }
}
