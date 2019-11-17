using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractableSceneObject
{
    public Transform parentStack;

    public bool Interact()
    {
        PlayerPickup.instance.PickupItem(this);
        return false;
    }

    public Transform GetThisTransform()
    {
        return transform;
    }
}
