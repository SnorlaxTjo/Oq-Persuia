using System;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform bossParent;
    [SerializeField] int[] amountOfWavesOnEachStage;

    [Header("EnemyPointers")]
    [SerializeField] Transform groundTransform;
    [SerializeField] float arrowGroundOffset;
    [SerializeField] GameObject enemyArrow;

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
    List<GameObject> allEnemyArrows = new List<GameObject>();

    public List<GameObject> AllEnemyArrows { get { return allEnemyArrows; } set { allEnemyArrows = value; } }

    private void Update()
    {
        if (allEnemyArrows.Count > 0)
        {
            foreach (GameObject arrow in allEnemyArrows)
            {
                arrow.transform.position = new Vector3(player.transform.position.x, groundTransform.position.y + arrowGroundOffset + 0.5f, player.transform.position.z);
            }
        }    
    }

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

                    GameObject summonedEnemyArrow = Instantiate(enemyArrow, bossParent);
                    summonedMinion.GetComponent<Minion>().CorrespondingArrow = summonedEnemyArrow;
                    allEnemyArrows.Add(summonedEnemyArrow);
                }
                break;

            case 1:
                switch (currentWave)
                {
                    case 0:
                        foreach(GameObject cannon in smallCannons)
                        {
                            cannon.GetComponent<BallParts>().Activated = true;
                            enemiesLeftOnCurrentWave++;

                            GameObject summonedEnemyArrow = Instantiate(enemyArrow, bossParent);
                            cannon.GetComponent<BallParts>().CorrespondingArrow = summonedEnemyArrow;
                            allEnemyArrows.Add(summonedEnemyArrow);
                        }
                        break;

                    case 1:
                        largeCannon.GetComponent<BallParts>().Activated = true;
                        enemiesLeftOnCurrentWave++;

                        GameObject summonedEnemyArrow2 = Instantiate(enemyArrow, bossParent);
                        largeCannon.GetComponent<BallParts>().CorrespondingArrow = summonedEnemyArrow2;
                        allEnemyArrows.Add(summonedEnemyArrow2);
                        break;
                }
                break;

            case 2:
                finalBoss.GetComponent<FinalBoss>().Activated = true;
                enemiesLeftOnCurrentWave++;
                break;
        }
    }
}

[Serializable]
public struct MinionSpawnLocations
{
    public Transform[] spawnLocation;
}
