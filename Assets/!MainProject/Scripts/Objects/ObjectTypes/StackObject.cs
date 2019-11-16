using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackObject : SceneObject
{
    [SerializeField] private GameObject items;

    private void Start()
    {
        GetItem().position = Vector3.zero;
    }

    public Transform GetItem()
    {
        GameObject item = Instantiate(items);
        item.GetComponent<Item>().parentStack = transform;

        return item.transform;
    }
}
