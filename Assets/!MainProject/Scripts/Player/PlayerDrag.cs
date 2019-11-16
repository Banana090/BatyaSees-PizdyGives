using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrag : MonoBehaviour
{
    public static PlayerDrag instance;

    [SerializeField] private float dragSpeedMultiplier;

    private Transform dragObject;
    private Interacting interactingScript;
    private PlayerMovement movementScript;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        interactingScript = GetComponent<Interacting>();
        movementScript = GetComponent<PlayerMovement>();
    }

    public void ReleaseDrag()
    {
        dragObject.SetParent(null);
        movementScript.moveSpeedMultiplier = 1f;
        dragObject = null;
    }

    public void SetDragObject(Transform obj)
    {
        dragObject = obj;
        dragObject.SetParent(transform);
        interactingScript.isDragging = true;
        movementScript.moveSpeedMultiplier = dragSpeedMultiplier;
    }
}
