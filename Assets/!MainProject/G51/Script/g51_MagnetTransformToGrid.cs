#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class g51_MagnetTransformToGrid : MonoBehaviour
{
    
    public float snapStep = 1f;

    private void LateUpdate()
    {
        if (Application.isPlaying)
            return;
        if (snapStep == 0) return;
        transform.localPosition = RoundVector(transform.localPosition, true);
        transform.localScale = RoundVector(transform.localScale);
    }



    Vector3 RoundVector(Vector3 vector, bool _pos = false)
    {
        float x = RoundFloat(vector.x, _pos);
        float y = RoundFloat(vector.y, _pos);
        float z = RoundFloat(vector.z, _pos);
        return new Vector3(x, y, z);
    }

    float RoundFloat(float value, bool _pos = false)
    {
        float g = _pos ? snapStep / 2f : snapStep;
        return Mathf.RoundToInt(value / g) * g;
    }
}
#endif
