using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    // Currently not utilized
    public static bool HasCollidedWithBottom(ContactPoint contactPoint)
    {
        bool hasCollidedWithBottom = true;
        if(contactPoint.thisCollider.bounds.center.y + contactPoint.thisCollider.gameObject.transform.position.y >= contactPoint.point.y)
        {
            hasCollidedWithBottom = false;
        }

        return hasCollidedWithBottom;
    }

    public static void PrintAllContactPoints(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint contactPoint = collision.GetContact(i);
            Debug.Log("Point[" + i + "]: " + contactPoint.point + ", Normal:" + contactPoint.normal + ", Separation: " + contactPoint.separation);
        }
    }
}
