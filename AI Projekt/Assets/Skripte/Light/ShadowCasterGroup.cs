using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[System.Serializable]
public class ShadowCasterGroup
{
    [SerializeField] private ShadowCasterType type;
    [SerializeField] private List<ShadowCasterProfileTag> profileTags;
    private ShadowCaster2D shadowCaster;

    public void CreateShadowCaster(CompositeCollider2D collider)
    {
        if (shadowCaster != null)
        {
            GameObject.Destroy(shadowCaster.gameObject);
        }
        ShadowCasterWallAdder adder = new ShadowCasterWallAdder();

        int pathCount = collider.pathCount;

        for (int i = 0; i < pathCount; ++i)
        {
            List<Vector2> pointsInPath = new List<Vector2>();
            List<Vector3> pointsInPath3D = new List<Vector3>();
            collider.GetPath(i, pointsInPath);

            for (int j = 0; j < pointsInPath.Count; ++j)
            {
                pointsInPath3D.Add(pointsInPath[j]);
                
            }
            switch (type)
            {
                case ShadowCasterType.lowerWall:
                    shadowCaster = adder.AddLowerWall(pointsInPath3D, collider);
                    break;
                case ShadowCasterType.upperWall:
                    shadowCaster = adder.AddUpperWall(pointsInPath3D, collider);
                    break;
                case ShadowCasterType.rightWall:
                    shadowCaster = adder.AddRightWall(pointsInPath3D, collider);
                    break;
                case ShadowCasterType.leftWall:
                    shadowCaster = adder.AddLeftWall(pointsInPath3D, collider);
                    break;
                case ShadowCasterType.filledWall:
                    shadowCaster = adder.AddFillInWall(pointsInPath3D, collider);
                    break;
            }
            
        }
    }

    public bool ContainsProfileTag(ShadowCasterProfileTag tag)
    {
        return profileTags.Contains(tag);
    }

    public ShadowCaster2D GetShadowCaster()
    {
        return shadowCaster;
    }
}

public enum ShadowCasterType
{
    lowerWall,
    upperWall,
    leftWall,
    rightWall,
    filledWall
}
