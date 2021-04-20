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
    private List<ShadowCaster2D> shadowCasters;

    public void CreateShadowCaster(CompositeCollider2D collider)
    {
        if (shadowCasters != null)
            foreach (ShadowCaster2D shadowCaster in shadowCasters)
                GameObject.Destroy(shadowCaster.gameObject);

        shadowCasters = new List<ShadowCaster2D>();
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
                    shadowCasters.Add(adder.AddLowerWall(pointsInPath3D, collider));
                    break;
                case ShadowCasterType.upperWall:
                    shadowCasters.Add(adder.AddUpperWall(pointsInPath3D, collider));
                    break;
                case ShadowCasterType.rightWall:
                    shadowCasters.Add(adder.AddRightWall(pointsInPath3D, collider));
                    break;
                case ShadowCasterType.leftWall:
                    shadowCasters.Add(adder.AddLeftWall(pointsInPath3D, collider));
                    break;
                case ShadowCasterType.filledWall:
                    shadowCasters.Add(adder.AddFillInWall(pointsInPath3D, collider));
                    break;
            }
            
        }
    }

    public bool ContainsProfileTag(ShadowCasterProfileTag tag)
    {
        return profileTags.Contains(tag);
    }

    public List<ShadowCaster2D> GetShadowCaster()
    {
        return shadowCasters;
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
