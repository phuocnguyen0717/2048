using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile State")]
public class TileState : ScriptableObject
{
    [SerializeField] Color backgroundColor;
    [SerializeField] Color textColor;
}
