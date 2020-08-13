using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementData))]
public class Paddle : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private Vector3 velocityTransferRates = Vector3.one;
    [Header("Dash")]
    [SerializeField] private float dashDistance = 1f;
    [SerializeField] private float dashSpeed = 15f;

    private Vector3 ballHeldOffset = Vector3.right;
    private int layerMasksColliding;
    private bool isDashing;

    private MovementData movementData;
    private Collider myCollider;

    private Ball heldBall;

    public Side PaddleSide
    {
        get; private set;
    }
    public bool IsBallHeld
    {
        get { return heldBall != null; }
    }
    public float CurrentSpeed
    {
        get { return baseSpeed; }
        private set { baseSpeed = value; }
    }

    public Vector3 WorldVelocity
    {
        get { return movementData.WorldVelocity; }
    }

    private void Awake()
    {
        layerMasksColliding = (1 << LayerManager.Default)  + (1 << LayerManager.Paddle) + (1 << LayerManager.Wall);
        isDashing = false;

        movementData = GetComponent<MovementData>();
        myCollider = GetComponent<Collider>();
    }

    public void SetPaddleSide(Side startingSide)
    {
        PaddleSide = startingSide;

        if(startingSide == Side.Right)
        {
            // Offset is based on the left paddle
            ballHeldOffset.x *= -1;
            // Spin the model around 180 degrees
            Renderer renderer = GetComponentInChildren<Renderer>();
            renderer.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
        }
    }

    public void SetHeldBall(Ball ball)
    {
        heldBall = ball;
        ball.StartFollowPaddle(this, ballHeldOffset);
    }
    
    // Will only shoot the ball if a ball is held
    public void ShootBall()
    {
        if (IsBallHeld)
        {
            Vector3 direction = Vector3.zero;
            if (PaddleSide == Side.Left)
            {
                direction = Vector3.right;
            }
            else if (PaddleSide == Side.Right)
            {
                direction = Vector3.left;
            }

            heldBall.Shoot(WorldVelocity, direction);

            // Stop holding the ball
            heldBall = null;
        }
    }

    public void Move(float input)
    {
        MovePaddleVerticalUnits(input * CurrentSpeed);
    }

    public void MovePaddleVerticalUnits(float deltaVertical)
    {
        Vector3 verticalDirection = deltaVertical >= 0 ? Vector3.up : Vector3.down;

        // Get the point on the cubical collider where the raycast's origin will be
        float raycastLength = Mathf.Abs(deltaVertical) + myCollider.bounds.extents.y;

        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, verticalDirection, out raycastHit, raycastLength, layerMasksColliding))
        {
            transform.position = raycastHit.point - verticalDirection * myCollider.bounds.extents.y;
        }
        else
        {
            transform.Translate(Vector3.up * deltaVertical);
        }
    }

    public Vector3 CalculateTransferedVelocity()
    {
        Vector3 transferedVelocity = new Vector3(velocityTransferRates.x * WorldVelocity.magnitude,
                                                 velocityTransferRates.y * WorldVelocity.magnitude,
                                                 velocityTransferRates.z * WorldVelocity.magnitude);
        return transferedVelocity;
    }

    public void ActivateDash(Side direction)
    {
        if (!isDashing)
        {
            int dashDirection = 1;
            if(direction == Side.Down)
            {
                dashDirection = -1;
            }

            isDashing = true;
            StartCoroutine(DashCoroutine(dashDistance, dashSpeed, dashDirection));
        }
    }

    private IEnumerator DashCoroutine(float dashDistance, float dashSpeed, int dashDirection)
    {
        //float u = 0f;
        //Vector3 dashPosition = Vector3.up * dashLength;
        //do
        //{
        //    Vector3.Lerp(Vector3.zero, dashPosition, u);
        //    yield return null;
        //    u += Time.deltaTime / dashDuration;
        //} while (u <= 1);

        float dashDistanceDone = 0;

        while (true)
        {
            float distanceToMove = dashSpeed * Time.deltaTime;

            if (distanceToMove > dashDistance - dashDistanceDone)
            {
                distanceToMove = dashDistance - dashDistanceDone;
            }

            MovePaddleVerticalUnits(distanceToMove * dashDirection);
            dashDistanceDone += distanceToMove;

            if(dashDistance == dashDistanceDone)
            {
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        isDashing = false;
    }
}
