using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Line
{
    const float verticalLineGradient = 1e5f;

    float gradient;
    float y_intercept;
    Vector2 pointOnLine_1;
    Vector2 pointOnLine_2;

    //float gradientPerpendicular;

    bool approachSide;

    public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
    {
        float dx = pointOnLine.x - pointPerpendicularToLine.x;
        float dy = pointOnLine.y - pointPerpendicularToLine.y;

        if (dy == 0) gradient = verticalLineGradient;
        else gradient = -dx / dy;

        // y = mx + b, where m is the gradient and b is the y-int
        // b = y - mx
        y_intercept = pointOnLine.y - gradient * pointOnLine.x;
        pointOnLine_1 = pointOnLine;
        pointOnLine_2 = pointOnLine + new Vector2(1, gradient);

        approachSide = false;
        approachSide = GetSide(pointPerpendicularToLine);
    }

    bool GetSide(Vector2 p)
    {
        return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
    }

    public bool HasCrossedLine(Vector2 p)
    {
        return GetSide(p) != approachSide;
    }

    public void DrawWithGizmos(float length)
    {
        Vector3 lineDir = new Vector3(1, -198, gradient).normalized;
        Vector3 lineCenter = new Vector3(pointOnLine_1.x, -198, pointOnLine_1.y) + Vector3.up;
        Gizmos.DrawLine(lineCenter - lineDir * length / 2f, lineCenter + lineDir * length / 2f);
    }
}