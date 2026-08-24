using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class FakeLoading : MonoBehaviour
{
    [SerializeField]
    private float duration = 2f;
    [SerializeField]
    private int Steps = 20;
    [SerializeField]
    private UnityEvent OnEndEvent;
    private Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        StartCoroutine(StartFakeLoading());
    }
    IEnumerator StartFakeLoading()
    {
        float valuePerStep = 1 /(float) Steps;
        float deleyPerStep = duration /(float) Steps;
        for (int i = 0; i < Steps; i++)
        {
            yield return new WaitForSeconds((Steps-i)*deleyPerStep);
            slider.value = slider.value + valuePerStep;
        }
        OnEndEvent?.Invoke();
    }
}
