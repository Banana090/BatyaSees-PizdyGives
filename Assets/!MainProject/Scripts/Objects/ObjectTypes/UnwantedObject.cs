using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnwantedObject : SceneObject, IInteractableSceneObject
{
    private void Start()
    {
        type = ObjectType.Unwanted;
    }

    public bool Interact()
    {
        Destroy(gameObject);
        return true;
    }

    public Transform GetThisTransform()
    {
        return transform;
    }
}
