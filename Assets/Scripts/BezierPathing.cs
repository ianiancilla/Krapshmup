using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPathing : MonoBehaviour
{
    [SerializeField] PathConfig path;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] bool looping = false;
    [SerializeField] [Tooltip("Variable or Fixed speed movement." +
                      "Variable will make object move faster on" +
                      "straight parts and slower on sharp turns.")]
                      bool variableSpeed = false;
    [SerializeField] [Tooltip("Only used for fixed speed movement")] 
                      int numWaypoints = 100;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        transform.position = path.GetPathCurves()[0][0].position;
        do
        {
            yield return StartCoroutine(FollowPath());
        }
        while (looping);
    }

    private IEnumerator FollowPath()
    {
        foreach (List<Transform> curve in path.GetPathCurves())
        {
            if (variableSpeed)
            {
                yield return StartCoroutine(FollowCurveVariableSpeed(curve));
            }
            else
            {
                yield return StartCoroutine(FollowCurveFixedSpeed(curve));
            }
        }
    }
    
    /// <summary>
    /// Moves object along a single bezier curve.
    /// Curve is calculated frame by frame in function of moveSpeed,
    /// resulting in object moving faster on straight parts and slower on curves
    /// </summary>
    /// <param name="curve">A list of 4 Transform objects, defining a bezier curve.
    /// Each is one of the lists inside a Path.GetPathCurves() list of lists.</param>
    private IEnumerator FollowCurveVariableSpeed(List<Transform> curve)
    {
        float tParam = 0f;
        Vector2 newPos;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * moveSpeed;

            newPos = Mathf.Pow(1 - tParam, 3) * curve[0].position +
                     3 * Mathf.Pow(1 - tParam, 2) * tParam * curve[1].position +
                     3 * (1 - tParam) * Mathf.Pow(tParam, 2) * curve[2].position +
                     Mathf.Pow(tParam, 3) * curve[3].position;

            transform.position = newPos;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FollowCurveFixedSpeed(List<Transform> curve)
    {
        // create list of waypoints based on bezier curve,  
        var waypointList = new List<Vector2>();
        for (float t = 0; t <= 1; t += (1f / numWaypoints))
        {
            var newPos = Mathf.Pow(1 - t, 3) * curve[0].position +
                             3 * Mathf.Pow(1 - t, 2) * t * curve[1].position +
                             3 * (1 - t) * Mathf.Pow(t, 2) * curve[2].position +
                             Mathf.Pow(t, 3) * curve[3].position;

            waypointList.Add(newPos);
        }

        // moves from waypoint to waypoint at fixed speed, yielding at each frame
        int currentWaypointIndex = 0;

        while(currentWaypointIndex <= waypointList.Count - 1)
        {
            var targetPos = waypointList[currentWaypointIndex];
            var deltaMove = moveSpeed * Time.deltaTime;
            
            if (Vector2.MoveTowards(transform.position,
                                                     targetPos,
                                                     deltaMove) != targetPos)
            {
                yield return transform.position = Vector2.MoveTowards(transform.position,
                                                     targetPos,
                                                     deltaMove);
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }
}
