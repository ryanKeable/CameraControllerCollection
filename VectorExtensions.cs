using System.Collections;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 ConvertToAnchorPos(this Vector2 viewportPoint)
    {
        return new Vector2((viewportPoint.x * Screen.width) - Screen.width / 2, (viewportPoint.y * Screen.height) - Screen.height / 2);
    }

    public static Vector2 Rotate(this Vector2 v, float delta)
    {
        delta *= Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }


    public static Vector2 Swizzle(this Vector2 v)
    {
        return new Vector2(
            v.y,
            v.x
        );
    }

    public static Vector3 SwizzleYXZ(this Vector3 v)
    {
        return new Vector3(
            v.y,
            v.x,
            v.z
        );
    }
    /// <summary>
    /// Set the y component of the vector to a value. Default is 0
    /// </summary>
    /// <param name="yVal">value to set (default is o)</param>
    /// <returns></returns>
    public static Vector3 SetY(this Vector3 v, float yVal = 0)
    {
        v = new Vector3(v.x, yVal, v.z);
        return v;
    }

    public static Vector3 FlattenY(this Vector3 v)
    { return v.SetY(0); }



    public static Vector2 Abs(this Vector2 v)
    {
        return new Vector2(
            Mathf.Abs(v.x),
            Mathf.Abs(v.y)
        );
    }

    public static Vector2 Pow(this Vector2 v, float exponent)
    {
        return new Vector2(
            Mathf.Pow(v.x, exponent),
            Mathf.Pow(v.y, exponent)
        );
    }

    public static Vector3 Pow(this Vector3 v, float exponent)
    {
        return new Vector3(
            Mathf.Pow(v.x, exponent),
            Mathf.Pow(v.y, exponent),
            Mathf.Pow(v.z, exponent)
        );
    }

    public static Vector4 Pow(this Vector4 v, float exponent)
    {
        return new Vector4(
            Mathf.Pow(v.x, exponent),
            Mathf.Pow(v.y, exponent),
            Mathf.Pow(v.z, exponent),
            Mathf.Pow(v.w, exponent)
        );
    }


    public static bool IsNaN(this Vector3 v)
    {
        return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
    }

    public static bool IsNaN(this Vector2 v)
    {
        return float.IsNaN(v.x) || float.IsNaN(v.y);
    }

}
