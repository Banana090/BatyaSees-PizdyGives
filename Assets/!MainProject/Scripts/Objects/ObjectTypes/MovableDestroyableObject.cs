using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableDestroyableObject : SceneObject, IInteractableSceneObject
{
    [Range(0f, 1f)]
    [SerializeField] private float repairPoints;
    [SerializeField] private Transform[] sprites;
    [SerializeField] private g51_Bar slider;

    private float repairAmount;

    public bool isBroken { get; private set; }
    public Vector3 startPosition { get; private set; }

    private void Start()
    {
        type = ObjectType.MovableDestroyable;
        startPosition = transform.position;
        RepairFully();
    }

    public void DestroyObject()
    {
        repairAmount = 0;
        sprites[0].gameObject.SetActive(false);
        sprites[1].gameObject.SetActive(true);
        isBroken = true;
    }

    public void Interact()
    {
        if (isBroken)
            Repair();
        else
            PlayerDrag.instance.SetDragObject(transform);
    }

    private void Repair()
    {
        repairAmount += repairPoints;
        slider.value = repairAmount;
        if (repairAmount >= 1)
            RepairFully();
        else
            slider.gameObject.SetActive(true);
    }

    private void RepairFully()
    {
        repairAmount = 1;
        sprites[0].gameObject.SetActive(true);
        sprites[1].gameObject.SetActive(false);
        isBroken = false;
        slider.gameObject.SetActive(false);
    }
}
