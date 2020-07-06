using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PaddleStartPosition
{
    Left,
    Right
}

[RequireComponent(typeof(MovementData))]
public class Paddle : MonoBehaviour
{
    [SerializeField] PaddleStartPosition startPosition = PaddleStartPosition.Left;
    [SerializeField] private float speed = 10f;

    private Vector3 ballHeldOffset = Vector3.right;

    private float maxYPosition;
    private float minYPosition;
    private MovementData movementData;

    private Ball heldBall;

    public PaddleStartPosition StartPosition
    {
        get { return startPosition; }
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
        SetBallOffsetSide();

        movementData = GetComponent<MovementData>();
    }

    private void Start()
    {
        SetYBoundaries();
    }

    public void HoldBall(Ball ball)
    {
        heldBall = ball;
        ball.StartFollowPaddle(this, ballHeldOffset);
    }

    public void ShootBall()
    {
        // If a ball is held
        if (heldBall != null)
        {
            Vector3 direction = Vector3.zero;
            if (startPosition == PaddleStartPosition.Left)
            {
                direction = Vector3.right;
            }
            else if (startPosition == PaddleStartPosition.Right)
            {
                direction = Vector3.left;
            }

            heldBall.Shoot(movementData.WorldVelocity, direction);
        }
    }

    public void MovePaddleVertically(float vertical)
    {
        float yPos = transform.position.y + vertical * Speed * Time.fixedDeltaTime;

        yPos = Mathf.Clamp(yPos, minYPosition, maxYPosition);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    private void SetYBoundaries()
    {
        float modelVerticalSize = GetComponent<Collider>().bounds.size.y;
        maxYPosition = LevelManager.instance.MaxYPosition - modelVerticalSize / 2;
        minYPosition = LevelManager.instance.MinYPosition + modelVerticalSize / 2;
    }

    private void SetBallOffsetSide()
    {
        if (startPosition == PaddleStartPosition.Right)
        {
            ballHeldOffset.x *= -1;
        }
    }
}
