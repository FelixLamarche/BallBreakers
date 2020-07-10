using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float initialHorizontalSpeed = 5f;
    [SerializeField] private Vector3 maxSpeedComponents = Vector3.one;

    private const float raycastLengthCollisionCheck = 8f;

    private int layerMasksToHit;
    private Vector3 paddleBallOffset;
    private Collider myCollider;

    [Header("Dynamic Properties")]
    [SerializeField] private Vector3 currentSpeed;
    private Paddle currentPaddle;
    private bool isMoving;
    private bool isFollowingPaddle;

    private void Awake()
    {
        isMoving = false;
        currentSpeed = Vector3.zero;
        layerMasksToHit = (1 << LayerManager.Default) + (1 << LayerManager.Ball) + (1 << LayerManager.Paddle) + (1 << LayerManager.Wall);
        myCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isFollowingPaddle)
        {
            FollowPaddle();
        }
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When hitting paddle, reverse x
        HitCollider(collision);
    }


    public void StopMoving()
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

        FollowPaddle();
    }

    public void Shoot(Vector3 throwerSpeed, Vector3 direction)
    {
        isMoving = true;
        isFollowingPaddle = false;

        currentSpeed = direction * initialHorizontalSpeed + throwerSpeed;
    }

    public void HitCollider(Collision collision)
    {
        // Get the normal, then reflect the speed vector
        ContactPoint contactPoint = collision.GetContact(0);
        Vector3 normal = contactPoint.normal;
        MovementData colliderMovementData = collision.gameObject.GetComponent<MovementData>();
        Vector3 AddedVelocity = Vector3.zero;
        if (colliderMovementData != null)
        {
            AddedVelocity = colliderMovementData.WorldVelocity;
        }

        Vector3 reflectedSpeed = Vector3.Reflect(currentSpeed, normal);
        SetCurrentSpeed(reflectedSpeed + AddedVelocity);
    }

    private void Move()
    {
        // NEW MOVE ALGORITHM
        // 1. Raycast from the edge of the collider from the point that will move with a distanceDelta of the distance to travel
            // Maybe do a first raycast of 1.5x max size of the bounds to get the starting point of the actual raycast
            // Or add the distance to the end of the collider to the raycast
        // 2. if no hit go to position
        // 3. If we hit
        // 4. Get the point of collision
        // 5. Get the position of our object outside of the collider in the direction we came from
        // 6. Check if the object can alter our move speed
        // 7. Redo from step 1 with the new velocity for the remaining distance

        // This gets the closest point to where should 
        Vector3 raycastStartPos = myCollider.bounds.ClosestPoint(transform.position + currentSpeed.normalized * myCollider.bounds.max.magnitude);
        float distanceToTravel = currentSpeed.magnitude * Time.fixedDeltaTime;

        RaycastHit raycastHit;
        if(Physics.Raycast(raycastStartPos, currentSpeed, out raycastHit, distanceToTravel, layerMasksToHit))
        {

        }

        CheckForCollisions();
        float xPos = transform.position.x + currentSpeed.x * Time.fixedDeltaTime;
        float yPos = transform.position.y + currentSpeed.y * Time.fixedDeltaTime;

        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }

    private void CheckForCollisions()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, currentSpeed, out raycastHit, raycastLengthCollisionCheck, layerMasksToHit))
        {
            Debug.Log(raycastHit.normal);
            Vector3 collPos = CalculatePositionOnCollision(raycastHit, currentSpeed);
        }
    }

    private Vector3 CalculatePositionOnCollision(RaycastHit raycastHit, Vector3 direction)
    {
        Vector3 collPoint = raycastHit.point;
        float yDelta = myCollider.bounds.center.y + myCollider.bounds.extents.y;
        float xDelta = myCollider.bounds.center.x + myCollider.bounds.extents.x;
        return collPoint;
    }

    private void SetCurrentSpeed(Vector3 newSpeed)
    {
        newSpeed.x = Mathf.Clamp(newSpeed.x, -maxSpeedComponents.x, maxSpeedComponents.x);
        newSpeed.y = Mathf.Clamp(newSpeed.y, -maxSpeedComponents.y, maxSpeedComponents.y);
        newSpeed.z = Mathf.Clamp(newSpeed.z, -maxSpeedComponents.z, maxSpeedComponents.z);

        currentSpeed = newSpeed;
    }

    private void FollowPaddle()
    {
        transform.position = currentPaddle.transform.position + paddleBallOffset;
    }
}
