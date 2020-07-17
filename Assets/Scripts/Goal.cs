using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Side goalSide = Side.Left;

    public void GoalScored()
    {
        // Inverse the goalSide to get the ScorerSide
        Side scorerSide = goalSide == Side.Left ? Side.Right : Side.Left;
        GameplayManager.LevelManager.GoalScored(scorerSide);
    }
}
