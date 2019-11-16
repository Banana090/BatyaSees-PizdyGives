using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ShowOdinSerializedPropertiesInInspector]
public sealed class MovableObject : SceneObject
{
    [PropertyRange(0f, 1f), SerializeField]
    public float speedPercent { get; private set; }
    public Vector2 startPosition { get; private set; }

    private void Start()
    {
        type = ObjectType.Movable;
        startPosition = transform.position;
    }

    public void Move(Direction direction, float speed)
    {
        Vector3 dir = Vector3.zero;

        switch (direction)
        {
            case Direction.Down: dir = Vector3.down; break;
            case Direction.Right: dir = Vector3.right; break;
            case Direction.Up: dir = Vector3.up; break;
            case Direction.Left: dir = Vector3.left; break;
        }

        transform.position += dir * speed * speedPercent * Time.deltaTime;
    }
}

public enum Direction
{
    Left,
    Right,
    Up,
    Down
}
