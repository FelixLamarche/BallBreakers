using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float initialHorizontalSpeed = 5f;

    private Paddle currentPaddle;
    private Vector3 paddleBallOffset;

    private bool isMoving;
    private bool isFollowingPaddle;
    private Vector3 currentSpeed;

    private void Awake()
    {
        isMoving = false;
        currentSpeed = Vector3.zero;
    }

    private void Update()
    {
        if (isFollowingPaddle)
        {
            transform.position = currentPaddle.transform.position + paddleBallOffset;
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
    }

    public void Shoot(Vector3 throwerSpeed, Vector3 direction)
    {
        isMoving = true;
        isFollowingPaddle = false;

        currentSpeed = direction * initialHorizontalSpeed + throwerSpeed;
    }

    public void HitWall()
    {
        // Reverse y speed vector
        currentSpeed.y *= -1;
    }

    public void HorizontalHit(Vector3 velocity)
    {
        // Reverse x speed vector
        currentSpeed.x *= -1.1f;
        currentSpeed += velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When hitting paddle, reverse x
        Paddle paddleHit = collision.gameObject.GetComponent<Paddle>();
        if(paddleHit != null)
        {
            HorizontalHit(paddleHit.WorldVelocity);
        }
        else if(collision.gameObject.tag == "Wall")
        {
            HitWall();
        }
    }
}
