using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackObject : SceneObject
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform[] itemVisuals;

    private int maxItems;
    private int itemsCount;

    private void Start()
    {
        type = ObjectType.Collectable;
        maxItems = itemVisuals.Length;
        itemsCount = maxItems;
    }

    [Sirenix.OdinInspector.Button]
    public void TestGetItem()
    {
        Transform item = GetItem();
        if (item != null)
            item.position = Vector3.zero;
    }

    public void PutItem()
    {
        itemsCount++;
        if (itemsCount > maxItems)
            itemsCount = maxItems;
        itemVisuals[itemsCount - 1].gameObject.SetActive(true);
    }

    public Transform GetItem()
    {
        if (itemsCount > 0)
        {
            GameObject item = Instantiate(itemPrefab);
            item.GetComponent<Item>().parentStack = transform;
            itemVisuals[itemsCount - 1].gameObject.SetActive(false);
            itemsCount--;

            return item.transform;
        }
        else
        {
            return null;
        }
    }
}
