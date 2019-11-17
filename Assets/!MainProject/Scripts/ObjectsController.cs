using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectsController : MonoBehaviour
{
    public static ObjectsController instance;

    [SerializeField] private MovableDestroyableObject[] barrels;
    [SerializeField] private Sprite[] barrelIcons;
    [SerializeField] private int barrelsOnLevel;

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

        sceneObjects.AddRange(FindObjectsOfType<SceneObject>().Where(x => x.gameObject.activeInHierarchy));
        for (int i = 0; i < sceneObjects.Count; i++)
            isObjectFree.Add(true);

        Stack<int> barrelIndexes = new Stack<int>();
        for (int i = 0; i < barrels.Length; i++)
            barrelIndexes.Push(i);

        barrelIndexes = new Stack<int>(barrelIndexes.OrderBy(x => Random.value));

        foreach (var item in barrels)
            item.gameObject.SetActive(false);

        for (int i = 0; i < barrelsOnLevel; i++)
            barrels[barrelIndexes.Pop()].gameObject.SetActive(true);

        Stack<int> iconIndexes = new Stack<int>();
        for (int i = 0; i < barrelIcons.Length; i++)
            iconIndexes.Push(i);

        iconIndexes = new Stack<int>(iconIndexes.OrderBy(x => Random.value));

        for (int i = 0; i < barrels.Length; i++)
        {
            if (barrels[i].gameObject.activeSelf)
                barrels[i].SetIcon(barrelIcons[iconIndexes.Pop()]);
        }
    }

    public static bool CheckForWin(out List<Transform> wrongObjects, int maxWrongItems)
    {
        wrongObjects = new List<Transform>();

        foreach (SceneObject obj in instance.sceneObjects)
        {
            switch (obj.type)
            {
                case ObjectType.Destroyable:
                    if ((obj as DestroyableObject).repairAmount < 1)
                        wrongObjects.Add(obj.transform);
                    break;

                case ObjectType.Movable:
                    MovableObject o = obj as MovableObject;
                    if ((o.startPosition - o.transform.position).magnitude > 0.35f)
                        wrongObjects.Add(obj.transform);
                    break;

                case ObjectType.MovableDestroyable:
                    MovableDestroyableObject mdo = obj as MovableDestroyableObject;
                    if (mdo.isBroken || (mdo.startPosition - mdo.transform.position).magnitude > 0.135f)
                        wrongObjects.Add(obj.transform);
                    break;

                case ObjectType.Trigger:
                    TriggerObject trg = obj as TriggerObject;
                    if (trg.currentIndex != trg.startIndex)
                        wrongObjects.Add(obj.transform);
                    break;
            }
        }

        Item[] i = FindObjectsOfType<Item>();

        foreach (var item in i)
            wrongObjects.Add(item.transform);

        UnwantedObject[] u = FindObjectsOfType<UnwantedObject>();

        foreach (var item in u)
            wrongObjects.Add(item.transform);

        return wrongObjects.Count <= maxWrongItems;
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
