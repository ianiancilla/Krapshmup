using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // variables
    WaveConfig waveConfig;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        var startPos = waveConfig.GetPath().GetPathCurves()[0][0].position;
        startPos = ApplyReverse(startPos);
        startPos = ApplyOffset(startPos);
        transform.position = startPos;

        do
        {
            yield return StartCoroutine(FollowPath());
        }
        while (waveConfig.GetLooping());
    }

    private IEnumerator FollowPath()
    {
        foreach (List<Transform> curve in waveConfig.GetPath().GetPathCurves())
        {
            if (waveConfig.GetVariableSpeed())
            {
                yield return StartCoroutine(FollowCurveVariableSpeed(curve));
            }
            else
            {
                yield return StartCoroutine(FollowCurveFixedSpeed(curve));
            }
        }
        if (! waveConfig.GetLooping())
        {
            Destroy(gameObject);
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
            tParam += Time.deltaTime * waveConfig.GetEnemySpeed();

            newPos = Mathf.Pow(1 - tParam, 3) * curve[0].position +
                     3 * Mathf.Pow(1 - tParam, 2) * tParam * curve[1].position +
                     3 * (1 - tParam) * Mathf.Pow(tParam, 2) * curve[2].position +
                     Mathf.Pow(tParam, 3) * curve[3].position;

            newPos = ApplyReverse(newPos);
            newPos = ApplyOffset(newPos);

            transform.position = newPos;

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FollowCurveFixedSpeed(List<Transform> curve)
    {
        // create list of waypoints based on bezier curve,  
        var waypointList = new List<Vector2>();
        for (float t = 0; t <= 1; t += (1f / waveConfig.GetNumWaypoints()))
        {
            var newPos = Mathf.Pow(1 - t, 3) * curve[0].position +
                             3 * Mathf.Pow(1 - t, 2) * t * curve[1].position +
                             3 * (1 - t) * Mathf.Pow(t, 2) * curve[2].position +
                             Mathf.Pow(t, 3) * curve[3].position;

            newPos = ApplyReverse(newPos);
            newPos = ApplyOffset(newPos);

            waypointList.Add(newPos);
        }

        // moves from waypoint to waypoint at fixed speed, yielding at each frame
        int currentWaypointIndex = 0;

        while(currentWaypointIndex <= waypointList.Count - 1)
        {
            var targetPos = waypointList[currentWaypointIndex];
            var deltaMove = waveConfig.GetEnemySpeed() * Time.deltaTime;

            var newPos = Vector2.MoveTowards(transform.position,
                                                     targetPos,
                                                     deltaMove);

            if (newPos != targetPos)
            {
                yield return transform.position = newPos;
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    /// <summary>
    /// Checks if position needs to be reflected due to wave having reverse on X or Y axes.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private Vector2 ApplyReverse(Vector2 pos)
    {
        if (waveConfig.GetReverseX())
        {
            pos = new Vector3(pos.x,
                              -(pos.y));
        }
        if (waveConfig.GetReverseY())
        {
            pos = new Vector3(-(pos.x),
                              pos.y);
        }
        return pos;
    }

    private Vector2 ApplyOffset(Vector2 pos)
    {
        float xOffset = waveConfig.GetXOffest();
        float yOffset = waveConfig.GetYOffest();

        if (xOffset != 0)
        {
            pos = new Vector3(pos.x + xOffset,
                              pos.y);
        }
        if (yOffset != 0)
        {
            pos = new Vector3(pos.x,
                              pos.y + yOffset);
        }
        return pos;
    }

}
