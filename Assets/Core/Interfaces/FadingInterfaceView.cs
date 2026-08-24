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

        if (target >= 0.9)
        {
            yield return new WaitForSecondsRealtime(StartupDelay);
            foreach (GameObject frame in Frames)
            {
                if (frame.TryGetComponent(out CanvasGroup group))
                {
                    group.alpha = 0;
                    frame.SetActive(true);
                    group.alpha = 0;
                }
                else
                {
                    frame.SetActive(true);
                }
            }
        }

        for (float i = 1; i <= Steps; i++)
        {
            yield return new WaitForSecondsRealtime(stepDuration);
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

        if (target <= 0.1)
        {
            foreach (GameObject frame in Frames)
            {
                frame.SetActive(false);
            }
        }
    }
}
