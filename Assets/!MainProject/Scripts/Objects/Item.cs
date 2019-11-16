using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractableSceneObject
{
    public Transform parentStack;

    public void Interact()
    {
        PlayerPickup.instance.PickupItem(this);
    }
}
