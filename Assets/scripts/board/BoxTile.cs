using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTile : MonoBehaviour {

    public int x;
    public int y;
    public bool inPlace;
    public BoxMask boxMask;

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
        inPlace = true;
    }

    public void Step(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 20);
        if(Vector3.Distance(transform.position, destination) < 0.02f)
        {
            transform.position = destination;
            inPlace = true;
        }
    }

    public bool IsTouched(Vector3 position, out BoxTile activeTile)
    {
        boxMask.UpdateRect();
        if (boxMask.HasPoint(position))
        {
            activeTile = this;
            return true;
        }
        activeTile = null;
        return false;
    }

    public bool HasSameValue(BoxTile boxTile)
    {
        return boxTile.GetValue().Equals(GetValue());
    }

    public Color GetValue()
    {
        return spriteRenderer.color;
    }

    public void RandomizeValue()
    {
        colorIndex = (int)(Random.Range(0f, 1f) * colors.Length);
        spriteRenderer.color = colors[colorIndex];
    }
}
