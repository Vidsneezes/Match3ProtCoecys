using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTile : MonoBehaviour {

    public int x;
    public int y;

    public int colorIndex;
    public SpriteRenderer spriteRenderer;

    private Color[] colors = new Color[]
    {
        Color.red,
        Color.blue,
        Color.green,
        Color.magenta,
        Color.yellow,
        Color.gray
    };

    public void Setup(Vector3 worldStartPos, Vector2 normalPosition, int _colorIndex)
    {
        x = (int)normalPosition.x;
        y = (int)normalPosition.y;

        transform.position = worldStartPos;
        spriteRenderer = GetComponent<SpriteRenderer>();
        colorIndex = _colorIndex;

        spriteRenderer.color = colors[colorIndex];

    }

    public bool HasSameValue(BoxTile boxTile)
    {
        return colorIndex == boxTile.colorIndex;
    }

    public Color GetValue()
    {
        return spriteRenderer.color;
    }
}
