using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementData : MonoBehaviour
{
    private Vector3 lastFixedFramePosition;
    private Vector3 currentFixedFramePosition;

    public Vector3 WorldVelocity
    {
        get { return (currentFixedFramePosition - lastFixedFramePosition) / Time.fixedDeltaTime; }
    }

    public void Start()
    {
        currentFixedFramePosition = transform.position;
    }

    private void FixedUpdate()
    {
        lastFixedFramePosition = currentFixedFramePosition;
        currentFixedFramePosition = transform.position;
    }
}
