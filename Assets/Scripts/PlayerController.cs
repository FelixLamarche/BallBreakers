using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Manages the player input for the paddle
[RequireComponent(typeof(Paddle))]
public class PlayerController : MonoBehaviour
{
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
        float vInput = Input.GetAxis("Vertical");

        if (vInput != 0)
        {
            verticalInputs.Add(vInput);
        }
    }

    private void GetShootingInput()
    {
        if (Input.GetButtonDown("Shoot") && paddle.IsBallHeld)
        {
            paddle.ShootBall();
        }
    }
}
