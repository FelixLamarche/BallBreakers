using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the player input for the paddle
[RequireComponent(typeof(Paddle))]
public class PlayerController : MonoBehaviour
{
    private Paddle paddle;

    private void Awake()
    {
        paddle = GetComponent<Paddle>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            paddle.ShootBall();
        }
    }

    private void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput != 0)
        {
            paddle.MovePaddleVertically(verticalInput);
        }
    }
}
