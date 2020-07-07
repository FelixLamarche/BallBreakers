using System;
using UnityEngine;

// Manages the current level
// Sets up the paddle, the ball and keeps track of the score
public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform leftPaddleStartTransform = null;
    [SerializeField] private Transform rightPaddleStartTransform = null;
    [SerializeField] private GameObject paddlePrefab = null;
    [SerializeField] private GameObject ballPrefab = null;

    private Ball ball;
    private Paddle leftPaddle;
    private Paddle rightPaddle;

    public int LeftPaddleScore
    {
        get; private set;
    }
    public int RightPaddleScore
    {
        get; private set;
    }

    private void Awake()
    {
        LeftPaddleScore = 0;
        RightPaddleScore = 0;

        SetLevel();
    }

    private void Start()
    {
        OnScoreChanged();
    }

    public void GoalScored(PaddleSide sideWhoScored)
    {
        PaddleSide paddleWhoLost = PaddleSide.Left;

        if (sideWhoScored == PaddleSide.Left)
        {
            paddleWhoLost = PaddleSide.Right;
            LeftPaddleScore++;
        }
        else if(sideWhoScored == PaddleSide.Right)
        {
            paddleWhoLost = PaddleSide.Left;
            RightPaddleScore++;
        }
        OnScoreChanged();

        SetRound(paddleWhoLost);
    }

    private void SetRound(PaddleSide sideWithBall)
    {
        Paddle ballHolderPaddle = null;

        if (sideWithBall == PaddleSide.Left)
        {
            ballHolderPaddle = leftPaddle;
        }
        else if(sideWithBall == PaddleSide.Right)
        {
            ballHolderPaddle = rightPaddle;
        }

        GiveBall(ballHolderPaddle);
    }

    private void SetLevel()
    {
        leftPaddle = CreatePaddle(PaddleSide.Left);
        rightPaddle = CreatePaddle(PaddleSide.Right);
        ball = CreateBall();

        SetRound(PaddleSide.Left);
    }

    private void GiveBall(Paddle paddle)
    {
        ball.Stop();
        paddle.SetHeldBall(ball);
    }

    private Paddle CreatePaddle(PaddleSide paddleSide)
    {
        Vector3 startingPosition;
        string objectName;
        bool isPlayer = false;

        if(paddleSide == PaddleSide.Left)
        {
            objectName = "LeftPaddle";
            isPlayer = true;
            startingPosition = leftPaddleStartTransform.position;
        }
        else
        {
            objectName = "RightPaddle";
            startingPosition = rightPaddleStartTransform.position;
        }

        GameObject paddleObject = Instantiate(paddlePrefab, startingPosition, Quaternion.identity);
        paddleObject.name = objectName;

        Paddle paddle = paddleObject.GetComponent<Paddle>();
        paddle.SetPaddle(paddleSide);

        if (isPlayer)
        {
            paddleObject.AddComponent<PlayerController>();
        }
        else if (!isPlayer)
        {
            paddleObject.AddComponent<ComputerController>();
        }

        return paddle;
    }

    private Ball CreateBall()
    {
        GameObject ballObject = Instantiate(ballPrefab);
        ballObject.name = "Ball";

        return ballObject.GetComponent<Ball>();
    }

    public event EventHandler ScoreChanged;

    private void OnScoreChanged()
    {
        ScoreChanged?.Invoke(this, EventArgs.Empty);
    }
}
