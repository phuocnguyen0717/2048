using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    public TileState[] tileStates;
    private TileGrid tileGrid;
    private List<Tile> tiles;
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
}
