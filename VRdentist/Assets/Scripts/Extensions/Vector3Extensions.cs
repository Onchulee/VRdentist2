using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static float ModularClamp(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        min += floor;
        max += floor;
        return Mathf.Clamp(angle, min, max);
    }
    
    public static Quaternion RotationClamp(Vector3 eulerAngles, Vector3 min, Vector3 max)
    {
        return Quaternion.Euler(
            ModularClamp(eulerAngles.x, min.x, max.x),
            ModularClamp(eulerAngles.y, min.y, max.y),
            ModularClamp(eulerAngles.z, min.z, max.z)
        );
    }

    public static Vector3 PositionClamp(Vector3 position, Vector3 min, Vector3 max)
    {
        return new Vector3(
            Mathf.Clamp(position.x, min.x, max.x),
            Mathf.Clamp(position.y, min.y, max.y),
            Mathf.Clamp(position.z, min.z, max.z)
        );
    }
}
