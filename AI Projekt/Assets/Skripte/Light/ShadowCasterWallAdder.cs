using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class ShadowCasterWallAdder
{
    private static Vector3 WALL_OFFSET_X = new Vector3(0.001f, 0);
    private static Vector3 WALL_OFFSET_Y = new Vector3(0, 0.001f);

    public ShadowCaster2D AddLeftWall(List<Vector3> pointsInPath3D, CompositeCollider2D collider)
    {
        List<Vector3> corners = GetCornersStartingFromLowerLeftWithOffset(pointsInPath3D);
        List<Vector3> leftWallPoints = new List<Vector3>() { corners[1] + WALL_OFFSET_X, corners[1], corners[0], corners[0] + WALL_OFFSET_X };
        return AddAditionalShadowCasterForWalls(leftWallPoints, collider);
    }

    public ShadowCaster2D AddRightWall(List<Vector3> pointsInPath3D, CompositeCollider2D collider)
    {
        List<Vector3> corners = GetCornersStartingFromLowerLeftWithOffset(pointsInPath3D);
        List<Vector3> rightWallPoints = new List<Vector3>() { corners[2] - WALL_OFFSET_X, corners[2], corners[3], corners[3] - WALL_OFFSET_X };
        return AddAditionalShadowCasterForWalls(rightWallPoints, collider);
    }

    public ShadowCaster2D AddLowerWall(List<Vector3> pointsInPath3D, CompositeCollider2D collider)
    {
        List<Vector3> corners = GetCornersStartingFromLowerLeftWithOffset(pointsInPath3D);
        List<Vector3> lowerWallPoints = new List<Vector3>() { corners[0], corners[0] - WALL_OFFSET_Y, corners[3] - WALL_OFFSET_Y, corners[3] };
        return AddAditionalShadowCasterForWalls(lowerWallPoints, collider);
    }

    public ShadowCaster2D AddUpperWall(List<Vector3> pointsInPath3D, CompositeCollider2D collider)
    {
        List<Vector3> corners = GetCornersStartingFromLowerLeftWithOffset(pointsInPath3D);
        List<Vector3> lowerWallPoints = new List<Vector3>() { corners[1], corners[1] - WALL_OFFSET_Y, corners[2] - WALL_OFFSET_Y, corners[2] };
        return AddAditionalShadowCasterForWalls(lowerWallPoints, collider);
    }

    public ShadowCaster2D AddFillInWall(List<Vector3> pointsInPath3D, CompositeCollider2D collider)
    {
        return AddShadowCaster(pointsInPath3D, collider);
    }

    private ShadowCaster2D AddAditionalShadowCasterForWalls(List<Vector3> wallPoints, CompositeCollider2D collider)
    {
        ShadowCaster2D wall = AddShadowCaster(wallPoints, collider);
        wall.selfShadows = true;
        return wall;
    }

    private ShadowCaster2D AddShadowCaster(List<Vector3> pointsInPath3D, CompositeCollider2D collider)
    {
        GameObject newShadowCaster = new GameObject("ShadowCaster2D" + pointsInPath3D.ToString());
        newShadowCaster.isStatic = true;
        newShadowCaster.transform.SetParent(collider.transform, false);
        ShadowCaster2D component = newShadowCaster.AddComponent<ShadowCaster2D>();
        component.selfShadows = true; //redundant
        SetPath(component, pointsInPath3D.ToArray());
        SetPathHash(component, Random.Range(int.MinValue, int.MaxValue));
        return component;
    }

    private List<Vector3> GetCornersStartingFromLowerLeftWithOffset(List<Vector3> pointsInPath3D)
    {
        List<Vector3> corners = new List<Vector3>();
        float maxX = pointsInPath3D[0].x;
        float maxY = pointsInPath3D[0].y;
        float minX = pointsInPath3D[0].x;
        float minY = pointsInPath3D[0].y;
        foreach (Vector3 point in pointsInPath3D)
        {
            maxX = Mathf.Max(maxX, point.x);
            maxY = Mathf.Max(maxY, point.y);
            minX = Mathf.Min(minX, point.x);
            minY = Mathf.Min(minY, point.y);
        }
        corners.Add(new Vector3(minX, minY, 0));
        corners.Add(new Vector3(minX, maxY, 0));
        corners.Add(new Vector3(maxX, maxY, 0));
        corners.Add(new Vector3(maxX, minY, 0));
        return corners;
    }

    public static void SetPath(ShadowCaster2D shadowCaster, Vector3[] path)
    {
        FieldInfo shapeField = typeof(ShadowCaster2D).GetField("m_ShapePath",
                                                               BindingFlags.NonPublic |
                                                               BindingFlags.Instance);
        shapeField.SetValue(shadowCaster, path);
    }

    public static void SetPathHash(ShadowCaster2D shadowCaster, int hash)
    {
        FieldInfo hashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash",
                                                              BindingFlags.NonPublic |
                                                              BindingFlags.Instance);
        hashField.SetValue(shadowCaster, hash);
    }
}
