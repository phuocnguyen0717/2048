using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileCell[] cells { get; private set; }
    public TileRow[] rows { get; private set; }
    public int size => cells.Length;
    public int height => rows.Length;
    public int width => size / height;
    void Awake()
    {
        cells = GetComponentsInChildren<TileCell>();
        rows = GetComponentsInChildren<TileRow>();
    }
    private void Start()
    {
        for (int y = 0; y < rows.Length; y++)
        {
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(y, x);
            }
        }
    }
}
