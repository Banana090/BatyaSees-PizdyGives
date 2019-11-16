using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameManager : SerializedMonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public LayerMask interactableObjects { get; private set; }
    [SerializeField] public int interactableLayerInt { get; private set; }

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);

        DontDestroyOnLoad(gameObject);
    }
}
