using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public static class ShadowCaster2DExtensions
{
    public static void SetPath(this ShadowCaster2D shadowCaster, Vector3[] path)
    {
        FieldInfo shapeField = typeof(ShadowCaster2D).GetField("m_ShapePath",
                                                               BindingFlags.NonPublic |
                                                               BindingFlags.Instance);
        shapeField.SetValue(shadowCaster, path);
    }

    public static void SetPathHash(this ShadowCaster2D shadowCaster, int hash)
    {
        FieldInfo hashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash",
                                                              BindingFlags.NonPublic |
                                                              BindingFlags.Instance);
        hashField.SetValue(shadowCaster, hash);
    }
}

public class ShadowCaster2DGenerator
{

}
