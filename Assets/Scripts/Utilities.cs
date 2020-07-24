using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static void PrintAllContactPoints(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint contactPoint = collision.GetContact(i);
            Debug.Log("Point[" + i + "]: " + contactPoint.point + ", Normal:" + contactPoint.normal + ", Separation: " + contactPoint.separation);
        }
    }
}
