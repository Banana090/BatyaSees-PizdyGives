using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SceneObject : SerializedMonoBehaviour
{
    public ObjectType type { get; protected set; }
}
