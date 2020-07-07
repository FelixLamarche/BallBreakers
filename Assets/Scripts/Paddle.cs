using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementData))]
public class Paddle : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private Vector3 ballHeldOffset = Vector3.right;

    private MovementData movementData;

    private Ball heldBall;

    public PaddleSide PaddleSide
    {
        get; private set;
    }
    public bool IsBallHeld
    {
        get { return heldBall != null; }
    }
    public float Speed
    {
        get { return speed; }
        private set { speed = value; }
    }

    public Vector3 WorldVelocity
    {
        get { return movementData.WorldVelocity; }
    }

    private void Awake()
    {
        movementData = GetComponent<MovementData>();
    }

    public void SetPaddle(PaddleSide startingSide)
    {
        PaddleSide = startingSide;
        CalculateBallOffsetBasedOnSide();
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
            if (PaddleSide == PaddleSide.Left)
            {
                direction = Vector3.right;
            }
            else if (PaddleSide == PaddleSide.Right)
            {
                direction = Vector3.left;
            }

            heldBall.Shoot(WorldVelocity, direction);

            // Stop holding the ball
            heldBall = null;
        }
    }

    public void MovePaddleVertically(float vertical)
    {
        float deltaYPos = vertical * Speed * Time.fixedDeltaTime;

        transform.Translate(Vector3.up * deltaYPos);
        //transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private void CalculateBallOffsetBasedOnSide()
    {
        // Offset is based on the left paddle
        if (PaddleSide == PaddleSide.Right)
        {
            ballHeldOffset.x *= -1;
        }
    }
}
