using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectsController : MonoBehaviour
{
    public static ObjectsController instance;

    private List<SceneObject> sceneObjects;
    private List<bool> isObjectFree; 

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        sceneObjects = new List<SceneObject>();
        isObjectFree = new List<bool>();

        // DEBUG ONLY. DELETE THIS
        for (int i = 0; i < transform.childCount; i++)
        {
            sceneObjects.Add(transform.GetChild(i).GetComponent<SceneObject>());
            isObjectFree.Add(true);
        }
    }

    public static SceneObject GetFreeObject()
    {
        List<int> freeIndexes = new List<int>();

        for (int i = 0; i < instance.isObjectFree.Count; i++)
        {
            if (instance.isObjectFree[i])
                freeIndexes.Add(i);
        }

        if (freeIndexes.Count > 0)
        {
            int index = freeIndexes[Random.Range(0, freeIndexes.Count)];
            instance.isObjectFree[index] = false;
            return instance.sceneObjects[index];
        }

        return null;
    }

    public static void FreeObject(SceneObject obj)
    {
        int index = instance.sceneObjects.IndexOf(obj);

        if (index < 0)
            throw new System.Exception("Пиздец, дядя, у вас возвращаемый SceneObject в ObjectSpawner не найден");

        instance.isObjectFree[index] = true;
    }
}
