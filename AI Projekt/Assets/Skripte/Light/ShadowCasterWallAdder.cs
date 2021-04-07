using System.Collections;
using System.Collections.Generic;
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
        List<Vector3> lowerWallPoints = new List<Vector3>() { corners[1], corners[1] - WALL_OFFSET_Y, corners[2] - WALL_OFFSET_Y, corners[2] };
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
        GameObject newShadowCaster = new GameObject("ShadowCaster2D");
        newShadowCaster.isStatic = true;
        newShadowCaster.transform.SetParent(collider.transform, false);
        ShadowCaster2D component = newShadowCaster.AddComponent<ShadowCaster2D>();
        component.SetPath(pointsInPath3D.ToArray());
        component.SetPathHash(Random.Range(int.MinValue, int.MaxValue));
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
            Debug.Log(point);
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
}
