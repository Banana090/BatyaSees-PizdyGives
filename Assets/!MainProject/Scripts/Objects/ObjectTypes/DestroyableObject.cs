using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : SceneObject, IInteractableSceneObject
{
    [Range(0f, 1f)]
    [SerializeField] private float repairPoints;
    [SerializeField] private Transform[] sprites;
    [SerializeField] private g51_Bar slider;

    public float repairAmount { get; private set; }

    private void Start()
    {
        type = ObjectType.Destroyable;
        RepairFully();
    }

    public void DestroyObject()
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
        gameObject.layer = 0;
        slider.gameObject.SetActive(false);
    }
}
