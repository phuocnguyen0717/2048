using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileCell[] cells { get; private set; }
    public TileRow[] rows { get; private set; }
    public int number;
}
