﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MovableObject : SceneObject, IInteractableSceneObject
{
    public Vector3 startPosition { get; private set; }

    private void Start()
    {
        type = ObjectType.Movable;
        startPosition = transform.position;
    }

    public bool Interact()
    {
        PlayerDrag.instance.SetDragObject(transform);
        return false;
    }

    public Transform GetThisTransform()
    {
        return transform;
    }
}