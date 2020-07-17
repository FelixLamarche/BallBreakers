using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float initialHorizontalSpeed = 5f;
    [SerializeField] private Vector3 maxSpeedComponents = Vector3.one;

    private int layerMasksToCollide;
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
        layerMasksToCollide = (1 << LayerManager.Default) + (1 << LayerManager.Ball) + (1 << LayerManager.Paddle) + (1 << LayerManager.Wall);
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
            Move(transform.position, Time.deltaTime);
        }
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

    // Sets the speed, while clamping its values
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

    private void Move(Vector3 currentPosition, float timeTravelling)
    {
        // NEW MOVE ALGORITHM
        // 1. Get the point on the collider that is situated in the direction of the currentSpeed vector starting at the center of the collider
        // 2. if nothing is hit, we go to the position
        // 3. If we hit
        // 4. Get the new position of our object based on the point of collision
        // 5. Get the time remaining for the movement of the frame
        // 6. Get the new speed
        // 7. Redo from step 1 with the new position, time and speed

        // Get the point on the cubical collider where the raycast's origin will be
        Vector3 colliderVerticesPoint = GetPointOnTheCubeColliderFromCenterTowardsDirection(currentSpeed, Vector3.right);
        Vector3 raycastStartPoint = currentPosition + colliderVerticesPoint;
        float distanceToTravel = currentSpeed.magnitude * timeTravelling;

        RaycastHit raycastHit;
        if (Physics.Raycast(raycastStartPoint, currentSpeed, out raycastHit, distanceToTravel, layerMasksToCollide))
        {
            Vector3 newPosition = raycastHit.point - colliderVerticesPoint;
            // The time remaining is based on the distance travelled with the currentSpeed
            float timeRemaining = timeTravelling * ((newPosition - currentPosition).magnitude / (currentSpeed * timeTravelling).magnitude);

            CheckForGoal(raycastHit.collider);

            // Set the new currentSpeed
            Vector3 AddedVelocity = CalculateAddedVelocityCollision(raycastHit.collider);
            Vector3 reflectedSpeed = Vector3.Reflect(currentSpeed, raycastHit.normal);
            SetCurrentSpeed(reflectedSpeed + AddedVelocity);

            Move(newPosition, timeRemaining);
        }
        else
        {
            float xPos = currentPosition.x + currentSpeed.x * timeTravelling;
            float yPos = currentPosition.y + currentSpeed.y * timeTravelling;
            transform.position = new Vector3(xPos, yPos, transform.position.z);
        }
    }

    private Vector3 CalculateAddedVelocityCollision(Collider collider)
    {
        MovementData colliderMovementData = collider.GetComponent<MovementData>();
        Vector3 AddedVelocity = Vector3.zero;
        if (colliderMovementData != null)
        {
            AddedVelocity = colliderMovementData.WorldVelocity;
        }

        return AddedVelocity;
    }

    // Returns a point on the cubical collider that intersects the direction vector that starts at the center of the collider
    private Vector3 GetPointOnTheCubeColliderFromCenterTowardsDirection(Vector3 direction, Vector3 axis)
    {
        float angleDirectionRightaxis = Vector3.SignedAngle(axis, direction, Vector3.forward);

        Vector3 collPoint = new Vector3(Mathf.Cos(angleDirectionRightaxis * Mathf.Deg2Rad), Mathf.Sin(angleDirectionRightaxis * Mathf.Deg2Rad), 0);

        // Because its a cube we need one of the sides to fully reach 1
        if (Mathf.Abs(angleDirectionRightaxis) > 45 && Mathf.Abs(angleDirectionRightaxis) < 135)
        {
            collPoint.y = 1 * Mathf.Sign(collPoint.y);
        }
        else
        {
            collPoint.x = 1 * Mathf.Sign(collPoint.x);
        }
        // Scale the colldierPointUnscaled to match the collider's bounds
        Vector3 collPointScaled = Vector3.Scale(collPoint, myCollider.bounds.extents);
        return collPointScaled;
    }

    private void CheckForGoal(Collider collider)
    {
        Goal goal = collider.GetComponent<Goal>();
        if (goal)
        {
            goal.GoalScored();
        }
    }
}
