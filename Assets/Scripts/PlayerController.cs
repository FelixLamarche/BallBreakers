using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Manages the player input for the paddle
[RequireComponent(typeof(Paddle))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private KeyCode upKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;
    [SerializeField] private KeyCode shootKey = KeyCode.D;
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift;

    private List<float> verticalInputs;
    private Paddle paddle;

    private void Awake()
    {
        verticalInputs = new List<float>();
        paddle = GetComponent<Paddle>();
    }

    private void Update()
    {
        GetVerticalInputs();
        GetShootingInput();

        if (Input.GetKeyDown(dashKey) && !paddle.IsDashing)
        {
            paddle.ActivateDash();
        }
    }

    private void FixedUpdate()
    {
        // This makes sure that every input is read
        if (verticalInputs.Count > 0)
        {
            float highestInput = verticalInputs.Max();
            paddle.Move(highestInput * Time.deltaTime);
            verticalInputs.Clear();
        }
    }

    private void GetVerticalInputs()
    {
        float vInput = 0;
        if (Input.GetKey(upKey))
        {
            vInput = 1;
        }
        else if (Input.GetKey(downKey))
        {
            vInput = -1;
        }

        if (vInput != 0)
        {
            verticalInputs.Add(vInput);
        }
    }

    private void GetShootingInput()
    {
        if (Input.GetKeyDown(shootKey) && paddle.IsBallHeld)
        {
            paddle.ShootBall();
        }
    }
}
