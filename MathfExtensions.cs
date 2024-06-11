using UnityEngine;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;

public static class MathfExtensions
{
    // NOTE! Always use this and not float.parse as float.parse will eat it if the thing being parsed is not a float
    // AND more importantly will not parse correctly in foreign locales where , == .
    [Pure]
    public static float parseFloat(object raw, float defaultFloat = 0f)
    {
        if (raw == null) return defaultFloat;
        float number = 0f;
        if (float.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out number)) {
            if (float.IsNaN(number)) return defaultFloat;
            return number;
        }
        return defaultFloat;
    }

    // NOTE! Always use this and not double.parse as double.parse will eat it if the thing being parsed is not a double
    // AND more importantly will not parse correctly in foreign locales where , == .
    [Pure]
    public static double parseDouble(object raw, double defaultDouble = 0d)
    {
        if (raw == null) return defaultDouble;
        double number = 0;
        if (double.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out number)) {
            return number;
        }
        return defaultDouble;
    }

    // NOTE! Always use this and not long.parse as long.parse will eat it if the thing being parsed is not a long
    // AND more importantly will not parse correctly in foreign locales where , == .
    [Pure]
    public static long parseLong(object raw, long defaultLong = 0L)
    {
        if (raw == null) return defaultLong;
        long number = 0;
        if (long.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out number)) {
            return number;
        }
        return defaultLong;
    }

    // NOTE! Always use this and not int.parse as int.parse will eat it if the thing being parsed is not a int
    // AND more importantly will not parse correctly in foreign locales where , == .
    [Pure]
    public static int parseInt(object raw, int defaultInt = 0)
    {
        if (raw == null) return defaultInt;
        int number = 0;
        if (int.TryParse(raw.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out number)) {
            return number;
        }
        return defaultInt;
    }

    // This is included mostly for completeness, not necessarily because it is math related
    [Pure]
    public static bool parseBool(object unknownType, bool defaultBool = false)
    {
        if (unknownType == null) return defaultBool;
        // Should be a string
        string unknownString = unknownType.ToString().ToLower();
        if (unknownString.Length == 0) return defaultBool;

        if (unknownString[0] == 'y') // yes
            return true;
        if (unknownString[0] == '1') // 0/1 binary
            return true;
        if (unknownString[0] == 't' && unknownString.Length == 4)
            return true;
        return false;
    }

    // This is also included mostly for completeness, not necessarily because it is math related.
    // Also, this hurts me since it uses reflection and is slow as fuck and if this code had a face I would punch it
    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T parseEnum<T>(string value) where T : struct, IConvertible
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T tryParseEnum<T>(string value, T defaultSetting) where T : struct, IConvertible
    {
        try {
            return (T)Enum.Parse(typeof(T), value, true);
        } catch {
            return defaultSetting;
        }
    }

    [Pure]
    public static Vector2 parseVector2(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return Vector2.zero;
        // Raw looks like x,y
        string[] tokens = raw.ToString().Split(',');
        if (tokens.Length != 2) return Vector2.zero;
        float x = parseFloat(tokens[0]);
        float y = parseFloat(tokens[1]);
        return new Vector2(x, y);
    }

    [Pure]
    public static Vector3 parseVector3(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return Vector3.zero;
        // Raw looks like x,y,z
        string[] tokens = raw.Split(',');
        if (tokens.Length != 3) return Vector3.zero;
        float x = parseFloat(tokens[0]);
        float y = parseFloat(tokens[1]);
        float z = parseFloat(tokens[2]);
        return new Vector3(x, y, z);
    }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double lerpDouble(double start, double end, double lerpValue)
    {
        double diff = (end - start) * lerpValue;
        return start + diff;
    }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static public float Map(float value, float istart, float istop, float ostart, float ostop)
    {
        return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
    }

    [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
    static public Vector3 Map(Vector3 value, Vector3 istart, Vector3 istop, Vector3 ostart, Vector3 ostop)
    {
        Vector3 result;
        result.x = ostart.x + ((ostop.x - ostart.x) * ((value.x - istart.x) / (istop.x - istart.x)));
        result.y = ostart.y + ((ostop.y - ostart.y) * ((value.y - istart.y) / (istop.y - istart.y)));
        result.z = ostart.z + ((ostop.z - ostart.z) * ((value.z - istart.z) / (istop.z - istart.z)));
        return result;
    }

    static public Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
    {
        Vector3 result;
        result.x = Mathf.Clamp(value.x, min.x, max.x);
        result.y = Mathf.Clamp(value.y, min.y, max.y);
        result.z = Mathf.Clamp(value.z, min.z, max.z);
        return result;
    }

    static public Vector3 Min(Vector3 a, Vector3 b)
    {
        Vector3 result;
        result.x = Mathf.Min(a.x, b.x);
        result.y = Mathf.Min(a.y, b.y);
        result.z = Mathf.Min(a.z, b.z);
        return result;
    }

    static public Vector3 Max(Vector3 a, Vector3 b)
    {
        Vector3 result;
        result.x = Mathf.Max(a.x, b.x);
        result.y = Mathf.Max(a.y, b.y);
        result.z = Mathf.Max(a.z, b.z);
        return result;
    }

    static public float Dist(float x1, float x2)
    {
        return Mathf.Abs(x2 - x1);// Mathf.Sqrt (Sq (x2 - x1));
    }

    static public float Dist(float x1, float y1, float x2, float y2)
    {
        return Mathf.Sqrt(Sq(x2 - x1) + Sq(y2 - y1));
    }

    static public float Dist(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        return Mathf.Sqrt(Sq(x2 - x1) + Sq(y2 - y1) + Sq(z2 - z1));
    }

    static public float Sq(float a)
    {
        return a * a;
    }


    public static bool Approximately(float a, float b)
    {
        float c = (b - a) < 0 ? -(b - a) : (b - a);
        a = (a < 0) ? -a : a;
        b = (b < 0) ? -b : b;


        a = 1E-06f * ((a <= b) ? b : a);

        b = (a <= 1.121039E-44f) ? 1.121039E-44f : a;

        return c < b;
    }


    public static float CloseToZero(float input)
    {
        return Mathf.Max(input, 1E-03f);
    }


    public static bool Approximately(Vector2 a, Vector2 b)
    {
        return MathfExtensions.Approximately(a.x, b.x) && MathfExtensions.Approximately(a.y, b.y);
    }

    public static bool Approximately(Vector3 a, Vector3 b)
    {
        return MathfExtensions.Approximately(a.x, b.x) && MathfExtensions.Approximately(a.y, b.y) && MathfExtensions.Approximately(a.z, b.z);
    }

    public static bool Approximately(Vector4 a, Vector4 b)
    {
        return MathfExtensions.Approximately(a.x, b.x) && MathfExtensions.Approximately(a.y, b.y) && MathfExtensions.Approximately(a.z, b.z) && MathfExtensions.Approximately(a.w, b.w);
    }

    public static bool Approximately(Color a, Color b)
    {
        return MathfExtensions.Approximately(a.r, b.r) && MathfExtensions.Approximately(a.g, b.g) && MathfExtensions.Approximately(a.b, b.b) && MathfExtensions.Approximately(a.a, b.a);
    }

    public static bool Approximately(Quaternion a, Quaternion b)
    {
        return MathfExtensions.Approximately(a.x, b.x) && MathfExtensions.Approximately(a.y, b.y) && MathfExtensions.Approximately(a.z, b.z) && MathfExtensions.Approximately(a.w, b.w);
    }
}
