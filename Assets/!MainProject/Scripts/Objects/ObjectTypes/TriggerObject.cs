﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TriggerObject : SceneObject, IInteractableSceneObject
{
    [SerializeField] private Transform[] states;

    private int startIndex;
    private int currentIndex;

    private void Start()
    {
        type = ObjectType.Trigger;
        startIndex = Random.Range(0, states.Length);

        currentIndex = startIndex;
        SetTrigger(currentIndex);
    }

    public void Trigger()
    {
        currentIndex++;
        if (currentIndex >= states.Length)
            currentIndex = 0;

        SetTrigger(currentIndex);
    }

    public void Interact()
    {
        Trigger();
    }

    private void SetTrigger(int index)
    {
        for (int i = 0; i < states.Length; i++)
            states[i].gameObject.SetActive(i == currentIndex);
    }
}