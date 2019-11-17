using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnwantedObject : SceneObject, IInteractableSceneObject
{
    private void Start()
    {
        type = ObjectType.Unwanted;
    }

    public void Interact()
    {
        Destroy(gameObject);
    }

    public Transform GetThisTransform()
    {
        return transform;
    }
}
