using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    // configuration variables
    [Header("Meta")]
    [TextArea(10, 14)] [SerializeField] string description;

    [Header("Wait Time")]
    [SerializeField] float timeBeforeWave = 0f;


    [Header("Enemy")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenSpawns = 0.5f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numOfEnemies = 5;
    [SerializeField] float enemySpeed = 2f;


    [Header("Path")]
    [SerializeField] PathConfig path;
    [SerializeField][Tooltip("Mirror path over X axes")]
                     bool reverseX = false;
    [SerializeField][Tooltip("Mirror path over Y axes")]
                     bool reverseY = false;

    [SerializeField][Tooltip("Does enemy disappear at the end of path," +
                             "or keeps looping over path")] 
                     bool looping = false;
    [SerializeField][Tooltip("Variable or Fixed speed movement." +
                     "Variable will make object move faster on" +
                     "straight parts and slower on sharp turns.")]
                    bool variableSpeed = false;
    [SerializeField][Tooltip("Only used for fixed speed movement")]
                    int numWaypoints = 300;
    [SerializeField] float xOffset = 0;
    [SerializeField] float yOffset = 0;


    public float GetTimeBeforeWave() { return timeBeforeWave; }

    public GameObject GetEnemyPrefab()    {return enemyPrefab;}

    public float GetTimeBetweenSpawns()    {return timeBetweenSpawns;}

    public float GetSpawnRandomFactor()    {return spawnRandomFactor;}

    public int GetNumOfEnemies()    {return numOfEnemies;}

    public float GetEnemySpeed()    {return enemySpeed;}

    public PathConfig GetPath()    { return path; }

    public bool GetLooping() { return looping; }

    public bool GetVariableSpeed() { return variableSpeed; }

    public int GetNumWaypoints() { return numWaypoints; }

    public bool GetReverseX() { return reverseX; }

    public bool GetReverseY() { return reverseY; }

    public float GetXOffest() { return xOffset; }

    public float GetYOffest() { return yOffset; }
}
