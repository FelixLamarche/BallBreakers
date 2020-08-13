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

    private void Update()
    {
        // FOR TEST ONLY
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SetRound(Side.Left);
        }
    }

    public void GoalScored(Side sideWhoLost)
    {
        if (sideWhoLost == Side.Left)
        {
            RightPaddleScore++;
        }
        else if(sideWhoLost == Side.Right)
        {
            LeftPaddleScore++;
        }
        else
        {
            Debug.LogError(sideWhoLost + " side who scored is Wrong");
        }
        OnScoreChanged();

        SetRound(sideWhoLost);
    }

    private void SetRound(Side sideWithBall)
    {
        Paddle ballHolderPaddle = null;

        if (sideWithBall == Side.Left)
        {
            ballHolderPaddle = leftPaddle;
        }
        else if(sideWithBall == Side.Right)
        {
            ballHolderPaddle = rightPaddle;
        }

        leftPaddle.transform.position = leftPaddleStartTransform.position;
        rightPaddle.transform.position = rightPaddleStartTransform.position;

        GiveBall(ballHolderPaddle);
    }

    private void SetLevel()
    {
        leftPaddle = CreatePaddle(Side.Left);
        rightPaddle = CreatePaddle(Side.Right);
        ball = CreateBall();

        SetRound(Side.Left);
    }

    private void GiveBall(Paddle paddle)
    {
        ball.StopMoving();
        paddle.SetHeldBall(ball);
    }

    private Paddle CreatePaddle(Side paddleSide)
    {
        Vector3 startingPosition;
        string objectName;
        bool isPlayer = false;

        if(paddleSide == Side.Left)
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
        paddle.SetPaddleSide(paddleSide);

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
