using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D))]
public class ShadowCasterGroupDefiner : MonoBehaviour
{
    public ShadowCasterProfileTag startUpTag;
    private CompositeCollider2D compositeCollider;
    [SerializeField] ShadowCasterGroup[] shadowCasterGroups;
    private List<ShadowCasterProfile> profiles;
    public OnEnterExitEvent events;

    private void Start()
    {
        compositeCollider = GetComponent<CompositeCollider2D>();
        CreateShadowCasters(compositeCollider);
        SetUpProfiles();
        events.onEnter += delegate { SetProfileActive(ShadowCasterProfileTag.InsideRoom); };
        events.onExit += delegate { SetProfileActive(ShadowCasterProfileTag.OutsideRoom); };
        SetProfileActive(startUpTag);
    }

    private void SetUpProfiles()
    {
        profiles = new List<ShadowCasterProfile>();
        foreach (ShadowCasterProfileTag tag in Enum.GetValues(typeof(ShadowCasterProfileTag)))
        {
            ShadowCasterProfile profile = new ShadowCasterProfile(tag);
            foreach (ShadowCasterGroup group in shadowCasterGroups)
            {
                if (group.ContainsProfileTag(tag))
                {
                    profile.AddShadowCaster(group.GetShadowCaster());
                }
            }
            profiles.Add(profile);
        }
    }

    private void CreateShadowCasters(CompositeCollider2D collider)
    {
        foreach (ShadowCasterGroup group in shadowCasterGroups)
        {
            group.CreateShadowCaster(collider);
        }
    }

    public void SetProfileActive(ShadowCasterProfileTag tag)
    {
        ShadowCasterProfile foundProfile = null;
        foreach (ShadowCasterProfile profile in profiles)
        {
            if (tag == profile.Tag)
                foundProfile = profile;
            else
                profile.SetShadowCasterActivity(false);
        }
        if (foundProfile!=null)
            foundProfile.SetShadowCasterActivity(true);
    }
}

public enum ShadowCasterProfileTag
{
    InsideRoom,
    OutsideRoom
}

