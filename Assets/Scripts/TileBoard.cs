using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    public TileState[] tileStates;
    private TileGrid tileGrid;
    private List<Tile> tiles;
    private bool waiting;
    void Awake()
    {
        tileGrid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }
    void Start()
    {
        CreateTile();
        CreateTile();
    }
    void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, tileGrid.transform);
        tile.SetState(tileStates[0], 2);
        tile.Spawn(tileGrid.GetRandomEmptyCell());
        tiles.Add(tile);
    }
    void Update()
    {
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveTiles(Vector2Int.down, 0, 1, tileGrid.Height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveTiles(Vector2Int.right, tileGrid.Width - 2, -1, 0, 1);
            }
        }
    }
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < tileGrid.Width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < tileGrid.Height; y += incrementY)
            {
                TileCell cell = tileGrid.GetCell(x, y);

                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }
        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = tileGrid.GetAdjacentCell(tile.cell, direction);
        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                break;
            }
            newCell = adjacent;
            adjacent = tileGrid.GetAdjacentCell(adjacent, direction);
        }
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }
        return false;
    }
    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;
    }

}
