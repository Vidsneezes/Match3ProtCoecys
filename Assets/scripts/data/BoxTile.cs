using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTile : MonoBehaviour {

    public int x;
    public int y;
    public BoxMask boxMask;
    public bool inPlace;

    private SpriteRenderer spriteRenderer;

    private Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.magenta };

	public void Setup(Vector3 worldStartPos, Vector2 normalPosition)
    {
        x = (int)normalPosition.x;
        y = (int)normalPosition.y;

        transform.position = worldStartPos;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = colors[(int)(Random.Range(0f, 1f) * 4)];
        inPlace = true;
    }

    public void Step(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime*15);
        if(Vector3.Distance(transform.position, destination) < 0.02f)
        {
            transform.position = destination;
            inPlace = true;
        }
    }
    
    public bool IsTouched(Vector3 mousePos, out BoxTile activeTile)
    {
        if (boxMask.HasPoint(mousePos))
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
        spriteRenderer.color = colors[(int)(Random.Range(0f, 1f) * 4)];

    }
}
