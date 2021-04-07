using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShadowCasterProfile
{
    public WallSide wallSide;
    public List<ShadowCaster2D> shadowCasters;

    public ShadowCasterProfile(WallSide _wallSide)
    {
        wallSide = _wallSide;
    }

    public void SetShadowCasterActivity(bool value)
    {
        foreach (ShadowCaster2D shadowCaster in shadowCasters)
        {
            if (shadowCaster != null)
            {
                shadowCaster.gameObject.SetActive(value);
            }
        }
    }
}

public enum WallSide
{
    Upper,
    Right,
    Lower,
    Left
}
