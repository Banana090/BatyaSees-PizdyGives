using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovableObject : SceneObject, IInteractableSceneObject
{
    public Vector2 startPosition { get; private set; }

    private void Start()
    {
        type = ObjectType.Movable;
        startPosition = transform.position;
    }

    public void Interact()
    {
        PlayerDrag.instance.SetDragObject(this);
    }
}