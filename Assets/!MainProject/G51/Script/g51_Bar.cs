using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class g51_Bar : MonoBehaviour
{
    public Transform scaler;
    [Range(0f, 1f)] public float value;

    private void LateUpdate()
    {
        Refresh();
    }

    void Refresh()
    {
        value = Mathf.Clamp(value, 0f, 1f);
        if (scaler)
            scaler.localScale = new Vector3(value, 1f, 1f);
    }
}
