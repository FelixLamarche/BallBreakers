using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private Side wallSide = Side.Up;

    public Side WallSide { get { return wallSide; } }
}
