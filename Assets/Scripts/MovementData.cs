using UnityEngine;

// Gets data on the gameObject's position and speed
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
