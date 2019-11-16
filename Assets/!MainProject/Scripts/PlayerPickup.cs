using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public static PlayerPickup instance;

    [SerializeField] private float distanceToStack;

    private Item pickedUpItem;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    public void PickupItem(Item item)
    {
        if (pickedUpItem == null)
        {
            pickedUpItem = item;
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.up * 1.2f;
        }
    }

    private void Update()
    {
        if (pickedUpItem != null)
        {
            if (Vector2.Distance(transform.position, pickedUpItem.parentStack.position) < distanceToStack)
            {
                //Call Game Manager, say its gone
                pickedUpItem.parentStack.GetComponent<StackObject>().PutItem();
                Destroy(pickedUpItem.gameObject);
                pickedUpItem = null;
            }
        }
    }
}
