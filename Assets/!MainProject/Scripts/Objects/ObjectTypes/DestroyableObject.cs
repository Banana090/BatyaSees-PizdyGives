using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : SceneObject, IInteractableSceneObject
{
    [Range(0f, 1f)]
    [SerializeField] protected float repairPoints;
    [SerializeField] protected Transform[] sprites;

    private float repairAmount;

    protected virtual void Start()
    {
        type = ObjectType.Destroyable;
        RepairFully();
    }

    public virtual void DestroyObject()
    {
        repairAmount = 0;
        sprites[0].gameObject.SetActive(false);
        sprites[1].gameObject.SetActive(true);
        gameObject.layer = GameManager.instance.interactableLayerInt;
    }

    public void Interact()
    {
        Repair();
    }

    protected void Repair()
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

    protected virtual void RepairFully()
    {
        repairAmount = 1;
        sprites[0].gameObject.SetActive(true);
        sprites[1].gameObject.SetActive(false);
        gameObject.layer = 0;
    }
}
