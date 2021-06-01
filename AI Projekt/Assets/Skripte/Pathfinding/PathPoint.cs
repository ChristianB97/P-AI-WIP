using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    [SerializeField] private PathPoint[] pathPoints;
    float lineThickness = 0.15f;
    private void OnDrawGizmos()
    {
        if (pathPoints!=null)
        {
            foreach(PathPoint pathpoint in pathPoints)
            {
                if (pathpoint != null)
                {
                    for (float i = -lineThickness; i <= lineThickness; i+=0.01f)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(new Vector3(transform.position.x+i, transform.position.y+i, transform.position.z), 
                            new Vector3(pathpoint.transform.position.x+i, pathpoint.transform.position.y+i, pathpoint.transform.position.z));
                    }
                    
                }
            }
        }
    }
}
