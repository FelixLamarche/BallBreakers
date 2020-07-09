using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private Side wallSide = Side.Up;
    [SerializeField] private Vector3 normal = Vector3.up;

    public Side WallSide { get { return wallSide; } }
    public Vector3 Normal { get { return normal; } }
}
