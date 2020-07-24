using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class is meant to control a Paddle to which it is attached, like PlayerController
[RequireComponent(typeof(Paddle))]
public class ComputerController : MonoBehaviour
{
    [SerializeField] private bool isFollowingBall = false;

    private Paddle paddle;
    private Ball ball;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    private void Start()
    {
        GetBallReference();
    }

    private void FixedUpdate()
    {
        if (isFollowingBall)
        {
            FollowBall();
        }
        if (paddle.IsBallHeld)
        {
            paddle.ShootBall();
        }
    }

    private void FollowBall()
    {
        float deltaY = ball.transform.position.y - transform.position.y;
        paddle.MovePaddleVerticalUnits(deltaY);
    }

    private void GetBallReference()
    {
        ball = FindObjectOfType<Ball>();
        if(ball == null)
        {
            Debug.LogError("Ball not found for CPU paddle :" + gameObject.name);
        }
    }
}
