using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using WaystoneMason.Agents;

[RequireComponent(typeof(WMAgent))]
public class ShipMover : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private bool Movement_Debugging_Toggle = true;
    private Camera targetCamera;
#endif
    [SerializeField]
    private PathConstructor PathConstructor;
    [SerializeField]
    private float GoalThreshold = 0.1f;
    [SerializeField]
    private float SlowDownDistance = 1f;
    [SerializeField]
    private float StartSpeed = 1f;
    [SerializeField]
    private float MaxSpeed = 10f;
    [SerializeField]
    private float SlowedSpeed;
    [SerializeField]
    private float Acceloration = 1f;
    private WMAgent agent;
    private bool moving = false;
    private float CurrentSpeed = 0f;
    private Vector2 goal;
    private Coroutine coroutine;
    private Coroutine constructorCoroutine;
    void Start()
    {
#if UNITY_EDITOR
        targetCamera = Camera.main;
#endif
        agent = GetComponent<WMAgent>();
    }
    void Update()
    {
        if (moving && goal != null)
        {
            agent.Speed = CurrentSpeed;
            float distance = Vector2.Distance(transform.position, goal);
            if (distance <= GoalThreshold)
            {
                moving = false;
                agent.Speed = 0;
                agent.SetGoal(transform.position);
                if (PathConstructor != null)
                {
                    PathConstructor.DestroyPath();
                }
            }

            if (distance > SlowDownDistance)
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, Time.deltaTime * Acceloration);
            } else
            {
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, SlowedSpeed, Time.deltaTime * Acceloration);
            }
        }
#if UNITY_EDITOR
        if (Movement_Debugging_Toggle && Mouse.current.leftButton.isPressed)
        {
            Vector2 point = targetCamera.ScreenToWorldPoint(Mouse.current.position.value);
            MoveAgent(point);
        }
#endif
    }
    public void MoveAgent(Vector2 target)
    {
        if (agent == null) return;
        if (constructorCoroutine != null) StopCoroutine(constructorCoroutine);
        if (coroutine != null) StopCoroutine(coroutine);
        coroutine = StartCoroutine(MoveCoroutine(target));
    }
    private IEnumerator MoveCoroutine(Vector2 target)
    {
        goal = target;
        agent.SetGoal(target);
        moving = false;
        agent.Speed = 0;
        if (PathConstructor != null)
        {
            constructorCoroutine = StartCoroutine(PathConstructor.BuildPathEnumerator(agent._path));
            yield return constructorCoroutine;
        }
        moving = true;
        CurrentSpeed = StartSpeed;
    }
}
