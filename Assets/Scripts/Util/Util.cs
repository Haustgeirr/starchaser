using UnityEngine;

public static class Util
{
    public static float CubicEaseOut(float t, float duration)
    {
        t /= duration;
        t--;

        return (t * t * t + 1);
    }

    public static float CubicEaseIn(float t, float duration)
    {
        t /= duration;

        return (t * t * t);
    }



    public static int mod(float a, float b)
    {
        return (int)(a - b * Mathf.Floor(a / b));
    }

}