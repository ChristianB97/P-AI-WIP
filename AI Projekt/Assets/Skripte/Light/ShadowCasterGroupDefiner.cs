using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TagDefiner))]
public class ShadowCasterGroupDefiner : MonoBehaviour
{
    private TagDefiner tagDefiner;
    [SerializeField] ShadowCasterGroup[] shadowCasterGroups;

    void Start()
    {
        tagDefiner = GetComponent<TagDefiner>();
    }

    public void OnValidate()
    {
        
    }

    private void InjectTagOptions()
    {
        foreach (ShadowCasterGroup shadowCasterGroup in shadowCasterGroups) 
        {
            
        }
    }
}
