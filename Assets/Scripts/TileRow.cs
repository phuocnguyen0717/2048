using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] cells { get; private set; }
    void Start()
    {
        cells = GetComponentsInChildren<TileCell>();
    }
}
