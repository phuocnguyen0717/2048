using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }
    public TileCell cell { get; private set; }
    public int number;
    public bool locked { get; set; }
    private Image backgroundColor;
    private TextMeshProUGUI text;
    [SerializeField] private Animation animationHelper;
    void Awake()
    {
        backgroundColor = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        animationHelper = FindObjectOfType<Animation>();
    }
    public void SetState(TileState state, int number)
    {
        this.state = state;
        this.number = number;

        backgroundColor.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = number.ToString();
    }
    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;
    }
    public void MoveTo(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = cell;
        this.cell.tile = this;
        Animate(cell.transform.position, false, 0.1f);
    }
    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }
        this.cell = null;
        cell.tile.locked = true;

        Animate(cell.transform.position, true, 0.1f);
    }
    private void Animate(Vector3 to, bool merging, float duration)
    {
        StartCoroutine(animationHelper.LerpPosition(transform, transform.position, to, duration));
        if (merging)
        {
            Destroy(gameObject);
        }
    }
}
