using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : SceneObject, IInteractableSceneObject
{
    [Range(0f, 1f)]
    [SerializeField] private float repairPoints;
    [SerializeField] private Transform[] sprites;

    private float repairAmount;

    private void Start()
    {
        type = ObjectType.Destroyable;
        RepairFully();

        DestroyObject(); // FOR TEST ONLY
    }

    public void DestroyObject()
    {
        repairAmount = 0;
        sprites[0].gameObject.SetActive(false);
        sprites[1].gameObject.SetActive(true);
    }

    public void Interact()
    {
        Repair();
    }

    private void Repair()
    {
        repairAmount += repairPoints;
        if (repairAmount >= 1)
        {
            RepairFully();
            //Repaired effect? Destroy slider
        }
        else
        {
            //ShowProgressSlider
        }
    }

    private void RepairFully()
    {
        repairAmount = 1;
        sprites[0].gameObject.SetActive(true);
        sprites[1].gameObject.SetActive(false);
    }
}
