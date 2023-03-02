using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindowBorderController
{
    public static Vector2 GetTopRightPosition(Camera camera)
    {
        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);

        return edgeVector;
    }

    public static Vector2 GetBottomLeftPosition(Camera camera)
    {
        Vector2 bottomLeftVector = new Vector2(0, 0);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(bottomLeftVector);

        return edgeVector;
    }
}
