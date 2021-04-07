using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShadowCasterFillInAdder : MonoBehaviour
{

    public bool isLeftWall, isRightWall, isUpperWall, isLowerWall, isFillInWall;
    private ShadowCasterProfile leftWallShadowCasterProfile, rightWallShadowCasterProfile, upperWallShadowCasterProfile, lowerWallShadowCasterProfile, fillInWallCasterProfile;

    public void Start()
    {
        CompositeCollider2D collider = GetComponent<CompositeCollider2D>();
        GenerateTilemapShadowCasters(collider);
    }

    public void GenerateTilemapShadowCasters(CompositeCollider2D collider)
    {

    }

}
