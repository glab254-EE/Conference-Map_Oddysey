using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingInterfaceView : AInterfaceView
{
    [SerializeField]
    private float StartupDelay = 0.5f;
    [SerializeField]
    private int Steps = 10;
    [SerializeField]
    private float Duration = 1.0f;
    private Coroutine currentCoroutine;
    protected override void Open()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeEnumerator(1));
    }

    protected override void Close()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(FadeEnumerator(0));
    }

    private IEnumerator FadeEnumerator(float target)
    {
        float stepDuration = Duration / Steps;
        List<GameObject> ignoredList = new();

        if (target >= 1)
        {
            yield return new WaitForSeconds(StartupDelay);
            foreach (GameObject frame in Frames)
            {
                frame.SetActive(true);
                if (frame.TryGetComponent(out CanvasGroup group))
                {
                    group.alpha = 0;
                }
            }
        }

        for (float i = 1; i <= Steps; i++)
        {
            yield return new WaitForSeconds(stepDuration);
            foreach(GameObject frame in Frames)
            {
                if (ignoredList.Contains(frame)) continue;
                if (frame.TryGetComponent(out CanvasGroup group))
                {
                    float next = Mathf.Lerp(1- target, target, Mathf.Clamp01(i / Steps));
                    group.alpha = next;
                } else
                {
                    ignoredList.Add(frame);
                }
            }
        }

        if (target <= 0)
        {
            foreach (GameObject frame in Frames)
            {
                frame.SetActive(false);
            }
        }
    }
}
