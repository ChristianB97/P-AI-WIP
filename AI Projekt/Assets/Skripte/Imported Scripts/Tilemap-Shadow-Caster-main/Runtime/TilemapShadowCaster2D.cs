﻿using System.Collections.Generic;
using UnityEngine;

namespace TilemapShadowCaster.Runtime
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("Rendering/2D/Tilemap Shadow Caster")]
    public class TilemapShadowCaster2D : MonoBehaviour
    {
        [SerializeField] private uint colliderHash;
        [SerializeField] private bool m_SelfShadows = false;
        
        private void Update()
        {
            CompositeCollider2D collider = GetComponent<CompositeCollider2D>();
            uint shapeHash = collider.GetShapeHash();
            if (shapeHash == colliderHash) return;
            colliderHash = shapeHash;
            ReinitializeShapes(collider);
        }

        private void ReinitializeShapes(CompositeCollider2D collider)
        {
            RemoveCurrentShadows();

            for (int i = 0; i < collider.pathCount; i++)
            {
                List<Vector2> points = new List<Vector2>();
                collider.GetPath(i, points);

                GameObject go = new GameObject("AutogeneratedShadowPath", typeof(MeshRenderer));
                go.transform.parent = transform;
                PathShadow path = go.AddComponent<PathShadow>();
                path.useRendererSilhouette = false;
                path.selfShadows = m_SelfShadows;
                path.SetShape(points);
            }
        }

        private void RemoveCurrentShadows()
        {
            new List<PathShadow>(GetComponentsInChildren<PathShadow>())
                .ConvertAll(comp => comp.transform.gameObject)
                .ForEach(gameObject =>
                {
                    if (Application.isEditor)
                    {
                        DestroyImmediate(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                });
        }

        public void ReinitializeShapes()
        {
            ReinitializeShapes(GetComponent<CompositeCollider2D>());
        }

        public void OnDestroy()
        {
            RemoveCurrentShadows();
        }
    }
}