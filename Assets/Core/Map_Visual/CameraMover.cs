using System;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [Serializable]
    public struct Bounds
    {
        public Vector2 Minimum;
        public Vector2 Maximum;
        public bool IsWithinRange(Vector2 input)
        {
            return input.x >= Minimum.x 
                && input.x <= Maximum.x
                && input.y >= Minimum.y 
                && input.y <= Maximum.y;
        }
        public Vector2 GetClampedV2(Vector2 input)
        {
            if (IsWithinRange(input)) return input;
            return new Vector2(
                Mathf.Clamp(input.x, Minimum.x, Maximum.x), 
                Mathf.Clamp(input.y, Minimum.y, Maximum.y)
                );
        }
    }
    [SerializeField]
    private float Move_Speed = 1f;
    [SerializeField]
    private Bounds Movement_Bounds;
    [SerializeField]
    private Transform Ship;
    [SerializeField]
    private float MoveThresshold = 0.05f;
    private float StartZ = 0f;
    void Start()
    {
        StartZ = transform.position.z;
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, Ship.position) >= MoveThresshold)
        {
            Vector2 _targetPos = Ship.position;
            if (!Movement_Bounds.IsWithinRange(_targetPos))
            {
                _targetPos = Movement_Bounds.GetClampedV2(Ship.position);
            }
            Vector2 lerped = Vector2.Lerp(transform.position, _targetPos, Move_Speed * Time.deltaTime);
            transform.position = new Vector3(lerped.x, lerped.y, StartZ);
        }
    }
}
