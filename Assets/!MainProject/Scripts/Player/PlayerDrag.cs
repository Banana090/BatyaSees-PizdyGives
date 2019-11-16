using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrag : MonoBehaviour
{
    public static PlayerDrag instance;

    [SerializeField] private float dragSpeedMultiplier;

    private MovableObject dragObject;
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
        dragObject.transform.SetParent(null);
        movementScript.moveSpeedMultiplier = 1f;
        dragObject = null;
    }

    public void SetDragObject(MovableObject obj)
    {
        dragObject = obj;
        dragObject.transform.SetParent(transform);
        interactingScript.isDragging = true;
        movementScript.moveSpeedMultiplier = dragSpeedMultiplier;
    }
}
