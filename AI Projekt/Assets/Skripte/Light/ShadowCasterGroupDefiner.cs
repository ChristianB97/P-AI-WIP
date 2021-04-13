using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CompositeCollider2D), typeof(OnEnterExitEvent))]
public class ShadowCasterGroupDefiner : MonoBehaviour
{
    private CompositeCollider2D compositeCollider;
    [SerializeField] ShadowCasterGroup[] shadowCasterGroups;
    private List<ShadowCasterProfile> profiles;
    public OnEnterExitEvent events;

    private void Start()
    {
        compositeCollider = GetComponent<CompositeCollider2D>();
        CreateShadowCasters(compositeCollider);
        SetUpProfiles();
        events.onExit += delegate { SetProfileActivity(ShadowCasterProfileTag.InsideRoom, false); };
        events.onEnter += delegate { SetProfileActivity(ShadowCasterProfileTag.OutsideRoom, false); };
        events.onEnter += delegate { SetProfileActivity(ShadowCasterProfileTag.InsideRoom, true); };
        events.onExit += delegate { SetProfileActivity(ShadowCasterProfileTag.OutsideRoom, true); };
        SetProfileActivity(ShadowCasterProfileTag.OutsideRoom, true);
        SetProfileActivity(ShadowCasterProfileTag.InsideRoom, false);
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

    public void SetProfileActivity(ShadowCasterProfileTag tag, bool isActive)
    {
        foreach (ShadowCasterProfile profile in profiles)
        {
            if (tag == profile.Tag)
            {
                profile.SetShadowCasterActivity(isActive);
            }
        }
    }
}

public enum ShadowCasterProfileTag
{
    InsideRoom,
    OutsideRoom
}

