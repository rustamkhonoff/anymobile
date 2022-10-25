using UnityEngine;
using Random = UnityEngine.Random;

public static class Utils
{
    public static float GetRandom(this Vector2 vector2)
    {
        return Random.Range(vector2.x, vector2.y);
    }
}