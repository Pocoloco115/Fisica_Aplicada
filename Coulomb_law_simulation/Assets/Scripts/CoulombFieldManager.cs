using System.Collections.Generic;
using UnityEngine;

public class CoulombFieldManager : MonoBehaviour
{
    public CoulombObject mainChargeObject;
    public float influenceRadius = 5f;
    public bool drawGizmos = true;
    public float minInteractionDistance = 0.5f;
    private static List<CoulombObject> allCharges = new List<CoulombObject>();

    void Start()
    {
        CoulombObject[] found = FindObjectsOfType<CoulombObject>();
        allCharges.AddRange(found);

        if (mainChargeObject == null)
        {
            foreach (var c in allCharges)
            {
                if (c.isMainCharge)
                {
                    mainChargeObject = c;
                    break;
                }
            }
        }
    }
    public static List<CoulombObject> GetAllCharges()
    {
        return allCharges;
    }
    public static void AddCharge(CoulombObject charge)
    {
        allCharges.Add(charge);
    }
    public static void RemoveCharge(CoulombObject charge)
    {
        allCharges.Remove(charge);
    }
    void FixedUpdate()
    {
        if (mainChargeObject == null) return;

        Vector2 mainPos = mainChargeObject.transform.position;
        float q1 = mainChargeObject.chargeValue;
        char s1 = mainChargeObject.chargeSign;

        foreach (var c in allCharges)
        {
            if (c == null || c == mainChargeObject) continue;

            Vector2 pos2 = c.transform.position;
            float dist = Vector2.Distance(mainPos, pos2);
            if (dist > influenceRadius) continue;
            if (dist < minInteractionDistance) continue; 

            float q2 = c.chargeValue;
            char s2 = c.chargeSign;

            Vector2 force = CoulombCalculatorUnity.GetForceVector(q1, s1, mainPos, q2, s2, pos2);
            c.ApplyCoulombForce(force);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!drawGizmos || mainChargeObject == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(mainChargeObject.transform.position, influenceRadius);
    }
}
