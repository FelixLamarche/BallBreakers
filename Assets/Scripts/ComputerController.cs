using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class is meant to control a Paddle to which it is attached, like PlayerController
[RequireComponent(typeof(Paddle))]
public class ComputerController : MonoBehaviour
{
    private Paddle paddle;
    private Ball ball;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    private void Start()
    {
        GetBall();
    }

    private void Update()
    {
        FollowBall();
        if (paddle.IsBallHeld)
        {
            paddle.ShootBall();
        }
    }

    private void FollowBall()
    {
        float newYPos = ball.transform.position.y;
        transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
    }

    private void GetBall()
    {
        ball = FindObjectOfType<Ball>();
        if(ball == null)
        {
            Debug.LogError("Ball not found for CPU padldle :" + gameObject.name);
        }
    }
}
