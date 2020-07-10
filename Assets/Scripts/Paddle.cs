using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementData))]
public class Paddle : MonoBehaviour
{
    [SerializeField] private float speed = 10f;

    private Vector3 ballHeldOffset = Vector3.right;

    private MovementData movementData;

    private Ball heldBall;

    public Side PaddleSide
    {
        get; private set;
    }
    public bool IsBallHeld
    {
        get { return heldBall != null; }
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
        movementData = GetComponent<MovementData>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerManager.Wall)
        {
            ContactPoint contactPoint = collision.GetContact(0);
            Wall wallComponent = contactPoint.otherCollider.GetComponent<Wall>();
            DontEnterCollider(contactPoint, wallComponent.Normal);
        }
    }


    public void SetPaddle(Side startingSide)
    {
        PaddleSide = startingSide;

        if(startingSide == Side.Right)
        {
            // Offset is based on the left paddle
            ballHeldOffset.x *= -1;
            // Spin the model around 180 degrees
            Renderer renderer = GetComponentInChildren<Renderer>();
            renderer.transform.rotation = Quaternion.Euler(Vector3.forward * 180);
        }
    }

    public void SetHeldBall(Ball ball)
    {
        heldBall = ball;
        ball.StartFollowPaddle(this, ballHeldOffset);
    }
    
    // Will only shoot the ball if a ball is held
    public void ShootBall()
    {
        if (IsBallHeld)
        {
            Vector3 direction = Vector3.zero;
            if (PaddleSide == Side.Left)
            {
                direction = Vector3.right;
            }
            else if (PaddleSide == Side.Right)
            {
                direction = Vector3.left;
            }

            heldBall.Shoot(WorldVelocity, direction);

            // Stop holding the ball
            heldBall = null;
        }
    }

    public void MovePaddleVertically(float vertical)
    {
        float deltaYPos = vertical * Speed;

        transform.Translate(Vector3.up * deltaYPos);
    }
    private void DontEnterCollider(ContactPoint contactPoint, Vector3 normal)
    {
        Vector3 toTranslate = normal * (-1 * contactPoint.separation);
        transform.Translate(toTranslate);
    }
    private void DontEnterCollider(ContactPoint contactPoint)
    {
        DontEnterCollider(contactPoint, contactPoint.normal);
    }
}
