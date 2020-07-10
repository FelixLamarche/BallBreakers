using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float initialHorizontalSpeed = 5f;
    [SerializeField] private Vector3 maxSpeedComponents = Vector3.one;

    private Paddle currentPaddle;
    private Vector3 paddleBallOffset;

    [Header("Dynamic Properties")]
    [SerializeField] private Vector3 currentSpeed;
    private bool isMoving;
    private bool isFollowingPaddle;

    private void Awake()
    {
        isMoving = false;
        currentSpeed = Vector3.zero;
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (isFollowingPaddle)
        {
            FollowPaddle();
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            float xPos = transform.position.x + currentSpeed.x * Time.fixedDeltaTime;
            float yPos = transform.position.y + currentSpeed.y * Time.fixedDeltaTime;

            transform.position = new Vector3(xPos, yPos, transform.position.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When hitting paddle, reverse x
        HitCollider(collision);
    }
    public void Stop()
    {
        isMoving = false;
        currentSpeed = Vector3.zero;
    }

    public void StartFollowPaddle(Paddle paddle, Vector3 offset)
    {
        isMoving = false;
        isFollowingPaddle = true;
        currentPaddle = paddle;
        paddleBallOffset = offset;

        FollowPaddle();
    }

    public void Shoot(Vector3 throwerSpeed, Vector3 direction)
    {
        isMoving = true;
        isFollowingPaddle = false;

        currentSpeed = direction * initialHorizontalSpeed + throwerSpeed;
    }

    public void HitCollider(Collision collision)
    {
        // Get the normal, then reflect the speed vector
        ContactPoint contactPoint = collision.GetContact(0);
        Vector3 normal = contactPoint.normal;
        MovementData colliderMovementData = collision.gameObject.GetComponent<MovementData>();
        Vector3 AddedVelocity = Vector3.zero;
        if (colliderMovementData != null)
        {
            AddedVelocity = colliderMovementData.WorldVelocity;
        }

        Vector3 reflectedSpeed = Vector3.Reflect(currentSpeed, normal);
        SetCurrentSpeed(reflectedSpeed + AddedVelocity);
    }

    private void SetCurrentSpeed(Vector3 newSpeed)
    {
        newSpeed.x = Mathf.Clamp(newSpeed.x, -maxSpeedComponents.x, maxSpeedComponents.x);
        newSpeed.y = Mathf.Clamp(newSpeed.y, -maxSpeedComponents.y, maxSpeedComponents.y);
        newSpeed.z = Mathf.Clamp(newSpeed.z, -maxSpeedComponents.z, maxSpeedComponents.z);

        currentSpeed = newSpeed;
    }

    private void FollowPaddle()
    {
        transform.position = currentPaddle.transform.position + paddleBallOffset;
    }
}
