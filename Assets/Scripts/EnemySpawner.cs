﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    // configuration variables
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] bool looping = false;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves());
        }
        while (looping);
    }

    private IEnumerator SpawnAllWaves()
    {
        foreach (WaveConfig waveConfig in waveConfigs)
        {
            yield return StartCoroutine(SpawnAllEnemiesInWave(waveConfig));
        }
    }

    private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        yield return new WaitForSeconds(waveConfig.GetTimeBeforeWave());

        for (int i = 0; i < waveConfig.GetNumOfEnemies(); i++)
        {
            var newEnemy = Instantiate(
                                        waveConfig.GetEnemyPrefab(),
                                        waveConfig.GetPath().GetPathCurves()[0][0].position,
                                        Quaternion.identity);
            newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
