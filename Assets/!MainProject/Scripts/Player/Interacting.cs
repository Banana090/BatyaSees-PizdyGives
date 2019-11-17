using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private Vector3 offset;

    [HideInInspector] public bool isDragging;

    private IInteractableSceneObject nearestInteractableObject;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForNearestObjects();

        if (Input.GetKeyDown(KeyCode.Space) && playerMovement.canMove)
        {
            if (!isDragging && nearestInteractableObject != null)
            {
                nearestInteractableObject.Interact();
            }
            else if (isDragging)
            {
                PlayerDrag.instance.ReleaseDrag();
                isDragging = false;
            }
        }

        if (!playerMovement.canMove && isDragging)
        {
            PlayerDrag.instance.ReleaseDrag();
            isDragging = false;
        }
    }

    private void CheckForNearestObjects()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position + offset, radius, GameManager.instance.interactableObjects);
        Collider2D nearestCollider;

        if (collisions.Length > 0)
        {
            nearestCollider = collisions[0];

            if (collisions.Length > 1)
            {
                float minDist = Vector2.Distance(transform.position + offset, nearestCollider.transform.position);

                for (int i = 1; i < collisions.Length; i++)
                {
                    float dist = Vector2.Distance(transform.position + offset, collisions[i].transform.position);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        nearestCollider = collisions[i];
                    }
                }
            }

            nearestInteractableObject = nearestCollider.GetComponent<IInteractableSceneObject>();
        }
        else
        {
            nearestInteractableObject = null;
        }
    }
}
