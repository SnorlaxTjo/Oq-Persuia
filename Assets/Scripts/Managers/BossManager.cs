using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] Transform bossParent;
    [SerializeField] int[] amountOfWavesOnEachStage;

    [Header("Stage 1 -  Minions")]
    [SerializeField] GameObject minion;
    [SerializeField] MinionSpawnLocations[] minionSpawnLocations;

    [Header("Stage 2 - Cannons")]
    [SerializeField] GameObject[] smallCannons;
    [SerializeField] GameObject largeCannon;

    [Header("Stage 3 - Final Boss")]
    [SerializeField] GameObject finalBoss;

    int currentStage;
    int currentWave;
    int enemiesLeftOnCurrentWave;

    public void RemoveEnemy()
    {
        enemiesLeftOnCurrentWave--;

        if (enemiesLeftOnCurrentWave < 1)
        {
            currentWave++;
            if (currentWave >= amountOfWavesOnEachStage[currentStage] && currentStage < amountOfWavesOnEachStage.Length)
            {
                currentStage++;
                currentWave = 0;
            }

            SummonNextWave();
        }
    }

    public void SummonNextWave()
    {
        switch (currentStage)
        {
            case 0:
                foreach(Transform location in minionSpawnLocations[currentWave].spawnLocation)
                {
                    GameObject summonedMinion = Instantiate(minion, bossParent);
                    summonedMinion.transform.position = location.position;
                    enemiesLeftOnCurrentWave++;
                }
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
}

[Serializable]
public struct MinionSpawnLocations
{
    public Transform[] spawnLocation;
}
