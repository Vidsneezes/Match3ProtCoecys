using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMask : MonoBehaviour {

    public Rect rect;

    public bool HasPoint(Vector3 point)
    {
        if (rect.Contains(point))
        {
            return true;
        }
        return false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        rect.position = new Vector2(transform.position.x - rect.width * 0.5f, transform.position.y - rect.height * 0.5f);

        Vector3 lowerLeft = new Vector3(rect.min.x, rect.min.y);
        Vector3 upperLeft = new Vector3(rect.min.x, rect.max.y);

        Vector3 lowerRight = new Vector3(rect.max.x, rect.min.y);
        Vector3 upperRight = new Vector3(rect.max.x, rect.max.y);

        Gizmos.DrawLine(lowerLeft, lowerRight);
        Gizmos.DrawLine(lowerLeft, upperLeft);
        Gizmos.DrawLine(upperLeft, upperRight);
        Gizmos.DrawLine(lowerRight, upperRight);
    }
}
