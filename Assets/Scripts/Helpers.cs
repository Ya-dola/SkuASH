using UnityEngine;

public static class Helpers
{
    // Adjust for Isometric Camera Angle
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45f, 0));

    public static Vector3 Vec3ToIso(this Vector3 vec3) => _isoMatrix.MultiplyPoint3x4(vec3);
}