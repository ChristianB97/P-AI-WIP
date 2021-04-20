using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class ShadowCasterProfile
{
    public ShadowCasterProfileTag Tag { get; private set; }
    public List<ShadowCaster2D> shadowCasters;

    public ShadowCasterProfile(ShadowCasterProfileTag _tag)
    {
        shadowCasters = new List<ShadowCaster2D>();
        Tag = _tag;
    }

    public void AddShadowCaster(List<ShadowCaster2D> _shadowCasters)
    {
        foreach (ShadowCaster2D shadowCaster in _shadowCasters)
        {
            shadowCasters.Add(shadowCaster);
        }
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
