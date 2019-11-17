using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject[] possibleUnwantedObjects;

    private SceneObject target;
    private Coroutine currentCoroutine;
    private float moveSpeed;
    private Transform item;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        moveSpeed = Random.Range(2.5f, 4f);
        StartCoroutine(UnwantedObjectsChance());
        StartCoroutine(StartWithDelay());
    }

    private IEnumerator StartWithDelay()
    {
        yield return new WaitForSeconds(1f);
        GoWork();
    }

    public void StopWorkingGoAway()
    {
        StopAllCoroutines();

        if (item != null)
        {
            item.SetParent(null);
            item.position = transform.position;
        }

        anim.SetBool("Run", false);
        anim.SetBool("Disappear", true);
        Destroy(gameObject, 2f);
    }

    public void GoWork()
    {
        target = ObjectsController.GetFreeObject();

        if (target == null)
        {
            anim.SetBool("Run", false);
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

                case ObjectType.Movable:
                    currentCoroutine = StartCoroutine(WorkWithMovable());
                    break;

                case ObjectType.MovableDestroyable:
                    currentCoroutine = StartCoroutine(WorkWithMovableDestroyable());
                    break;
            }
        }
    }

    private IEnumerator UnwantedObjectsChance()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.6f);
            if (Random.Range(0f, 1f) < 0.032f)
                Instantiate(possibleUnwantedObjects[Random.Range(0, possibleUnwantedObjects.Length)], transform.position, Quaternion.identity);
        }
    }

    private IEnumerator WorkWithMovableDestroyable()
    {
        anim.SetBool("Run", true);

        MovableDestroyableObject mdo = target as MovableDestroyableObject;

        if (mdo.isBroken)
        {
            anim.SetBool("Run", false);
            ObjectsController.FreeObject(target);
            GoWork();
            yield break;
        }

        while (Vector2.Distance(transform.position, target.transform.position) > 0.6f)
        {
            Vector3 moveDir = (target.transform.position - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Sign(moveDir.x), 1, 1);
            yield return null;
        }

        if (Random.Range(0f, 1f) < 0.9f)
        {
            Vector3 offset = target.transform.position - transform.position;
            Vector3 moveToPoint = GameManager.GetRandomPointInRoom();
            Vector3 moveTo = (moveToPoint - transform.position).normalized;

            while (Vector2.Distance(transform.position, moveToPoint) > 0.5f)
            {
                transform.position += moveTo * moveSpeed * 0.6f * Time.deltaTime;
                target.transform.position = transform.position + offset;
                yield return null;
            }

            anim.SetBool("Run", false);

            if (Random.Range(0f, 1f) < 0.2f)
            {
                yield return new WaitForSeconds(Random.Range(1f, 1.6f));
                mdo.DestroyObject();
            }

        } else
        {
            anim.SetBool("Run", false);
            yield return new WaitForSeconds(Random.Range(1f, 1.6f));
            mdo.DestroyObject();
        }

        ObjectsController.FreeObject(target);
        GoWork();
    }

    private IEnumerator WorkWithMovable()
    {
        anim.SetBool("Run", true);
        while (Vector2.Distance(transform.position, target.transform.position) > 0.6f)
        {
            Vector3 moveDir = (target.transform.position - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Sign(moveDir.x), 1, 1);
            yield return null;
        }

        Vector3 offset = target.transform.position - transform.position;
        Vector3 moveToPoint = GameManager.GetRandomPointInRoom();
        Vector3 moveTo = (moveToPoint - transform.position).normalized;

        while (Vector2.Distance(transform.position, moveToPoint) > 0.5f)
        {
            transform.position += moveTo * moveSpeed * 0.6f * Time.deltaTime;
            target.transform.position = transform.position + offset;
            yield return null;
        }
        anim.SetBool("Run", false);
        ObjectsController.FreeObject(target);
        GoWork();
    }

    private IEnumerator WorkWithCollectable()
    {
        StackObject stack = target as StackObject;
        anim.SetBool("Run", true);

        while (Vector2.Distance(transform.position, target.transform.position) > 1f)
        {
            Vector3 moveDir = (target.transform.position - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Sign(moveDir.x), 1, 1);
            yield return null;
        }

        item = stack.GetItem();
        if (item == null) yield break;

        item.SetParent(transform);
        item.localPosition = Vector3.up * 1.2f;

        Vector3 targetPoint = GameManager.GetRandomPointInRoom();

        while (Vector2.Distance(transform.position, targetPoint) > 0.8f)
        {
            Vector3 moveDir = (targetPoint - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Sign(moveDir.x), 1, 1);
            yield return null;
        }

        item.SetParent(null);
        item.position = transform.position;
        item = null;

        anim.SetBool("Run", false);
        ObjectsController.FreeObject(target);
        GoWork();
    }

    private IEnumerator WorkWithDestroyable()
    {
        DestroyableObject destroyable = target as DestroyableObject;

        anim.SetBool("Run", true);
        while (Vector2.Distance(transform.position, target.transform.position) > 1f)
        {
            Vector3 moveDir = (target.transform.position - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Sign(moveDir.x), 1, 1);
            yield return null;
        }

        anim.SetBool("Run", false);
        yield return new WaitForSeconds(Random.Range(1f, 1.6f));

        destroyable.DestroyObject();
        ObjectsController.FreeObject(target);
        GoWork();
    }

    private IEnumerator WorkWithTrigger()
    {
        TriggerObject trigger = target as TriggerObject;

        anim.SetBool("Run", true);
        while (Vector2.Distance(transform.position, target.transform.position) > 1f)
        {
            Vector3 moveDir = (target.transform.position - transform.position).normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Sign(moveDir.x), 1, 1);
            yield return null;
        }

        anim.SetBool("Run", false);
        
        trigger.Trigger();
        ObjectsController.FreeObject(target);
        GoWork();
    }
}
