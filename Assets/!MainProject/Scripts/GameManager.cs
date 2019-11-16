using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ShowOdinSerializedPropertiesInInspector]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public LayerMask interactableObjects;
    [SerializeField] public LayerMask movableObjects;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
    }
}
