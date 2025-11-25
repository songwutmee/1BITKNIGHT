using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DamageVignetteUI : MonoBehaviour
{
    [Header("Fade Settings")]
    [Range(0f, 1f)]
    public float maxAlpha = 0.8f;
    public float fadeSpeed = 4f;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0; 
    }

    public void TriggerEffect()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        //Fade In
        while (_canvasGroup.alpha < maxAlpha)
        {
            _canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        _canvasGroup.alpha = maxAlpha;

        //Fade Out 
        while (_canvasGroup.alpha > 0f)
        {
            _canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        _canvasGroup.alpha = 0f;
    }
}