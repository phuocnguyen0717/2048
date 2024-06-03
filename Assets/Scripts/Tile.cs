using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileCell cell { get; private set; }
    public int number;
    public TileState tileState { get; private set; }
    private Image backgroundColor;
    private TextMeshProUGUI text;

    void Awake()
    {
        backgroundColor = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

    }
    public void SetState(TileState state, int number)
    {
        this.tileState = state;
        this.number = number;

        backgroundColor.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = number.ToString();
    }
}
