using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private Ball ball;
    private Paddle paddleLeft;
    private Paddle paddleRight;

    // The y points based on the camera's view point
    private float maxYPosition;
    private float minYPosition;

    public float MaxYPosition
    {
        get { return maxYPosition; }
    }

    public float MinYPosition
    {
        get { return minYPosition; }
    }

    void Awake()
    {
        instance = this;
        SetLevelBoundaries();
    }

    private void Start()
    {
        ball = FindObjectOfType<Ball>();

        Paddle[] paddles = FindObjectsOfType<Paddle>();
        if(paddles[0].StartPosition == PaddleStartPosition.Left)
        {
            paddleLeft = paddles[0];
            paddleRight = paddles[1];
        }
        else
        {
            paddleLeft = paddles[1];
            paddleRight = paddles[0];
        }

        paddleLeft.HoldBall(ball);
    }

    private void SetLevelBoundaries()
    {
        if (Camera.main.orthographic)
        {
            maxYPosition = Camera.main.orthographicSize + Camera.main.transform.position.y;
            minYPosition = -Camera.main.orthographicSize + Camera.main.transform.position.y;
        }
        else
        {
            Debug.LogError("Camera is not Orthographic");
        }
    }
}
