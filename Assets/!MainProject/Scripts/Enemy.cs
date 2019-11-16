using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject[] possibleUnwantedObjects;

    private SceneObject target;
    private Coroutine currentCoroutine;
    private float moveSpeed;

    private void Start()
    {
        moveSpeed = Random.Range(1f, 3f);
        StartCoroutine(UnwantedObjectsChance());
        GoWork();
    }

    public void StopWorkingGoAway()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        //Go to exit, dissapear;
        Destroy(gameObject);
    }

    public void GoWork()
    {
        target = ObjectsController.GetFreeObject();

        if (target == null)
        {
            return;
        }
        else
        {
            switch (target.type)
            {
                case ObjectType.Trigger:
                    currentCoroutine = StartCoroutine(WorkWithTrigger());
                    break;

                case ObjectType.Destroyable:
                    currentCoroutine = StartCoroutine(WorkWithDestroyable());
                    break;

                case ObjectType.Collectable:
                    currentCoroutine = StartCoroutine(WorkWithCollectable());
                    break;
            }
        }
    }

    private IEnumerator UnwantedObjectsChance()
    {
        //while (true)
        {
            yield return new WaitForSeconds(1f);
            if (Random.Range(0f, 1f) < 0.02f)
                Instantiate(possibleUnwantedObjects[Random.Range(0, possibleUnwantedObjects.Length)], transform.position, Quaternion.identity);
        }
    }

    private IEnumerator WorkWithCollectable()
    {
        StackObject stack = target as StackObject;

        while (Vector2.Distance(transform.position, target.transform.position) > 1f)
        {
            transform.position += (target.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }

        Transform item = stack.GetItem();
        if (item == null) yield break;

        item.SetParent(transform);
        item.localPosition = Vector3.up * 1.2f;

        Vector3 targetPoint = GameManager.GetRandomPointInRoom();

        while (Vector2.Distance(transform.position, targetPoint) > 0.5f)
        {
            transform.position += (targetPoint - transform.position).normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }

        item.SetParent(null);
        item.position = transform.position;

        ObjectsController.FreeObject(target);
        GoWork();
    }

    private IEnumerator WorkWithDestroyable()
    {
        DestroyableObject destroyable = target as DestroyableObject;

        while (Vector2.Distance(transform.position, target.transform.position) > 1f)
        {
            transform.position += (target.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(1.5f, 3f));

        destroyable.DestroyObject();
        ObjectsController.FreeObject(target);
        GoWork();
    }

    private IEnumerator WorkWithTrigger()
    {
        TriggerObject trigger = target as TriggerObject;

        while (Vector2.Distance(transform.position, target.transform.position) > 1f)
        {
            transform.position += (target.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            yield return null;
        }

        float workingTime = Random.Range(1f, 2.5f);
        float timeStartedWorking = Time.time;

        while(timeStartedWorking + workingTime >= Time.time)
        {
            trigger.Trigger();
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
        ObjectsController.FreeObject(target);
        GoWork();
    }
}
