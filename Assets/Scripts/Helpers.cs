using UnityEngine;

public static class Helpers
{
    // Adjust for Isometric Camera Angle
    // private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 90f, 0));

    // public static Vector3 Vec3ToIso(this Vector3 vec3) => _isoMatrix.MultiplyPoint3x4(vec3);

    public static float RandomRangePlusMin(this float range) => Random.Range(-range, range);

    public static float GetCosineAngle(float yPosDiff, float amplitude, float frequency)
    {
        //  Nathf.Acos Only works from -1 to 1
        return Mathf.Acos(yPosDiff / amplitude >= 1 ? 1 : yPosDiff / amplitude) / frequency;
    }
}