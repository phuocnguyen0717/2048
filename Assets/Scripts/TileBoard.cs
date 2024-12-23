using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    public TileState[] tileStates;
    private TileGrid tileGrid;
    private List<Tile> tiles;
    [SerializeField] public TextMeshProUGUI scoreText;
    [SerializeField] public TextMeshProUGUI bestScoreText;

    private bool waiting;
    void Awake()
    {
        tileGrid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }
    private void Start()
    {
        bestScoreText.text = GameManager.LoadBestScore().ToString();
    }
    void Update()
    {
        HandleInputAndMoveTiles();
    }
    public void ClearBoard()
    {
        foreach (var cell in tileGrid.cells)
        {
            cell.tile = null;
        }
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }
    public void CreateTile()
    {
        Tile tile = Instantiate(tilePrefab, tileGrid.transform);
        tile.SetState(tileStates[0], 2);
        tile.Spawn(tileGrid.GetRandomEmptyCell());
        tiles.Add(tile);
        AnimateTile(tile);
    }
    private void HandleInputAndMoveTiles()
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
                if (CanMerge(tile, adjacent.tile))
                {
                    Merge(tile, adjacent.tile);
                    return true;
                }
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
    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }
    private void OnEnable()
    {

        GameManager.OnScoreChanged += UpdateScoreUI;
    }
    private void OnDisable()
    {

        GameManager.OnScoreChanged -= UpdateScoreUI;
    }
    private void UpdateScoreUI(int newScore)
    {
        scoreText.text = newScore.ToString();
        Debug.Log("Score Updated: " + newScore);
    }
    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);


        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        int number = b.number * 2;
        b.SetState(tileStates[index], number);
        AnimateTile(b);
        GameManager.Instance.IncreaseScore(number);
    }
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }
        return -1;
    }
    private IEnumerator WaitForChanges()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }
        if (tiles.Count != tileGrid.Size)
        {
            CreateTile();
        }
        if (CheckForGameOver())
        {
            GameManager.Instance.GameOver();
        }
    }
    private bool CheckForGameOver()
    {
        if (tiles.Count != tileGrid.Size)
        {
            return false;
        }
        foreach (var tile in tiles)
        {
            TileCell up = tileGrid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = tileGrid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = tileGrid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = tileGrid.GetAdjacentCell(tile.cell, Vector2Int.right);
            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }
            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }
            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }
            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }
        return true;
    }

    private void AnimateTile(Tile tile)
    {
        float originalScale = tile.transform.localScale.x;
        float targetScale = originalScale * 1.5f;
        float duration = 0.005f;

        tile.transform.DOScale(targetScale, duration).SetLoops(2, LoopType.Yoyo);
    }
}
