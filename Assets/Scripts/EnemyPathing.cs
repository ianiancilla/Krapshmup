using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    // variables
    WaveConfig waveConfig;

    int waypointIndex = 0;
    List<Transform> waypoints;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWayPoints();

        PositionAtStartWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToNextWaypoint();
    }

    private void PositionAtStartWaypoint()
    {
        transform.position = waypoints[0].position;
    }

    private void MoveToNextWaypoint()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPos = waypoints[waypointIndex].position;
            var deltaMove = waveConfig.GetEnemySpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, 
                                                    targetPos,
                                                    deltaMove);

            if (transform.position == waypoints[waypointIndex].position)    // if waypoint reached, go to next one
            {
                waypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

}
