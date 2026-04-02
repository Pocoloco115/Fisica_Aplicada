using UnityEngine;

public static class CoulombCalculatorUnity
{
    public const float k = 8.9875517923e9f;

    public static float CalculateForceMagnitude(float q1, float q2, Vector2 pos1, Vector2 pos2)
    {
        Vector2 diff = pos2 - pos1;
        float r2 = diff.sqrMagnitude;
        if (r2 == 0f)
            return 0f;
        return k * (q1 * q2) / r2;
    }

    public static string GetForceDirection(char sign1, char sign2)
    {
        return sign1 == sign2 ? "Repulsive" : "Attractive";
    }

    public static Vector2 GetForceVector(
        float q1, char sign1, Vector2 pos1,
        float q2, char sign2, Vector2 pos2)
    {
        float force = CalculateForceMagnitude(q1, q2, pos1, pos2);
        if (force == 0f) return Vector2.zero;

        string direction = GetForceDirection(sign1, sign2);

        Vector2 diff = pos2 - pos1;
        float distance = diff.magnitude;
        if (distance == 0f) return Vector2.zero;

        Vector2 dirUnit = diff / distance;

        if (direction == "Attractive")
            dirUnit = -dirUnit;

        return dirUnit * force;
    }
}