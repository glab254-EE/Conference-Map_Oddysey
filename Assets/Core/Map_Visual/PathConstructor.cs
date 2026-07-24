using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class PathConstructor : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;
    [SerializeField] private float DistanceBetweenPrefabs;
    [SerializeField] private float PerInstanceDelay;
    private List<GameObject> currentPath;
    private Coroutine currentCoroutine;
    public void BuildNewPath(List<Vector2> path)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(BuildPathEnumerator(path));
    }
    public void DestroyPath()
    {
        if (currentPath != null)
        {
            for (int i = currentPath.Count-1; i>=0; i--)
            {
                Destroy(currentPath[i]);
            }
        }
        currentPath = null;
    }
    public IEnumerator BuildPathEnumerator(List<Vector2> path)
    {
        DestroyPath();
        currentPath = new();
        yield return null;
        Vector2? lastPoint = null;
        List<Vector2> ExsistingPathsAtPosition = new();
        float remainingDistance = 0f;
        if (path == null)
        {
            yield break;
        }
        foreach (var point in path) 
        {
            if (lastPoint != null)
            {
                float baseDistance = Vector2.Distance(lastPoint.Value, point);
                if (baseDistance + remainingDistance >= DistanceBetweenPrefabs)
                {
                    int count = Mathf.CeilToInt(baseDistance / DistanceBetweenPrefabs);
                    remainingDistance = baseDistance % DistanceBetweenPrefabs;
                    if (count > 1)
                    {
                        for (float i = 0; i < count; i++)
                        {
                            float a = i / count;
                            Vector2 subPoint = Vector2.LerpUnclamped(lastPoint.Value, point, a);
                            if (ExsistingPathsAtPosition.Contains(subPoint))
                            {
                                continue;
                            }
                            GameObject newObj = Instantiate(Prefab);
                            newObj.transform.position = subPoint;
                            newObj.transform.up = (point - lastPoint.Value).normalized;
                            currentPath.Add(newObj);
                            ExsistingPathsAtPosition.Add(subPoint);
                            yield return new WaitForSeconds(PerInstanceDelay);
                        }
                    } else if (count == 1)
                    {
                        Vector2 subPoint = Vector2.LerpUnclamped(lastPoint.Value, point, 0.5f);
                        if (ExsistingPathsAtPosition.Contains(subPoint))
                        {
                            continue;
                        }
                        GameObject newObj = Instantiate(Prefab);
                        newObj.transform.position = subPoint;
                        newObj.transform.up = (point - lastPoint.Value).normalized;
                        currentPath.Add(newObj);
                        ExsistingPathsAtPosition.Add(subPoint);
                        yield return new WaitForSeconds(PerInstanceDelay);
                    }
                }
            }
            lastPoint = point;
        }
    }
}
