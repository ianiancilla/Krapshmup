using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    // configuration variables
    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numOfEnemies = 5;
    [SerializeField] float enemySpeed = 2f;


    [Header("Path")]
    [SerializeField] PathConfig path;
    [SerializeField][Tooltip("Does enemy disappear at the end of path," +
                             "or keeps looping over path")] 
                     bool looping = false;
    [SerializeField][Tooltip("Variable or Fixed speed movement." +
                     "Variable will make object move faster on" +
                     "straight parts and slower on sharp turns.")]
                    bool variableSpeed = false;
    [SerializeField][Tooltip("Only used for fixed speed movement")]
                    int numWaypoints = 300;


    public GameObject GetEnemyPrefab()    {return enemyPrefab;}

    public float GetTimeBetweenSpawns()    {return timeBetweenSpawns;}

    public float GetSpawnRandomFactor()    {return spawnRandomFactor;}

    public int GetNumOfEnemies()    {return numOfEnemies;}

    public float GetEnemySpeed()    {return enemySpeed;}

    public PathConfig GetPath()    { return path; }

    public bool GetLooping() { return looping; }

    public bool GetVariableSpeed() { return variableSpeed; }

    public int GetNumWaypoints() { return numWaypoints; }
}
