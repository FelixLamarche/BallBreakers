using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private PaddleSide goalSide = PaddleSide.Left;

    private void OnTriggerEnter(Collider other)
    {
        // If a ball enters the goal
        if (other.gameObject.layer == LayerManager.Ball)
        {
            // line works if there are two sides in PaddleSide
            PaddleSide scorerSide = (PaddleSide) (((int) goalSide + 1) % 2);
            GameplayManager.LevelManager.GoalScored(scorerSide);
        }
    }
}
