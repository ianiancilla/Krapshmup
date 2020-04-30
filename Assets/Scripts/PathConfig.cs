using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SplinePaths/Pathconfig")]
public class PathConfig : ScriptableObject
{
    // config variables
    [SerializeField] [Tooltip("First point of a curve must be the same as the previous' last.")] 
                     GameObject[] curvePrefabs;

    /// <summary>
    /// Will return a list of curves in the path, each curve is a list of waypoints, as Transform objects.
    /// </summary>
    public List<List<Transform>> GetPathCurves()
    {
        var pathCurves = new List<List<Transform>>();

        foreach (GameObject curve in curvePrefabs)
        {
            var waypoints = new List<Transform>();
            foreach (Transform child in curve.transform)
            {
                waypoints.Add(child);
            }
            pathCurves.Add(waypoints);
        }
        
        return pathCurves;
    }


}
