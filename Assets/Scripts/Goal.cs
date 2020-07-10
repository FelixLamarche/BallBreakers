using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Side goalSide = Side.Left;

    private void OnTriggerEnter(Collider other)
    {
        // If a ball enters the goal
        if (other.gameObject.layer == LayerManager.Ball)
        {
            // Only works with two goals on these sides
            Side scorerSide = goalSide == Side.Left ? Side.Right : Side.Left;
            GameplayManager.LevelManager.GoalScored(scorerSide);
        }
    }
}
