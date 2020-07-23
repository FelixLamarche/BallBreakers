using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementData))]
public class Paddle : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 10f;

    private Vector3 ballHeldOffset = Vector3.right;
    private int layerMasksToCollide;

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
        layerMasksToCollide = (1 << LayerManager.Default)  + (1 << LayerManager.Paddle) + (1 << LayerManager.Wall);

        movementData = GetComponent<MovementData>();
        myCollider = GetComponent<Collider>();
    }

    public void SetPaddle(Side startingSide)
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
        if (Physics.Raycast(transform.position, verticalDirection, out raycastHit, raycastLength, layerMasksToCollide))
        {
            transform.position = raycastHit.point - verticalDirection * myCollider.bounds.extents.y;
        }
        else
        {
            transform.Translate(Vector3.up * deltaVertical);
        }
    }
}
