using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MiniBoss : MonoBehaviour
{
    [SerializeField] GameObject player;

    [Space]

    [SerializeField] GameObject enemy;
    [SerializeField] Transform[] enemySpawnLocations;

    [Header("EnemyPointers")]
    [SerializeField] Transform groundTransform;
    [SerializeField] float arrowGroundOffset;
    [SerializeField] GameObject enemyArrow;

    [Space]

    [SerializeField] UnityEvent whatToDoUponVictory;

    int enemiesLeft;
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

    public void SpawnEnemies()
    {
        foreach(Transform location in enemySpawnLocations)
        {
            GameObject spawnedEnemy = Instantiate(enemy, transform.parent);
            spawnedEnemy.transform.position = location.position;
            spawnedEnemy.GetComponent<Minion>().ForMiniBossfight = true;
            enemiesLeft++;

            GameObject summonedEnemyArrow = Instantiate(enemyArrow, transform.parent);
            spawnedEnemy.GetComponent<Minion>().CorrespondingArrow = summonedEnemyArrow;
            allEnemyArrows.Add(summonedEnemyArrow);
        }
    }

    public void RemoveEnemy()
    {
        enemiesLeft--;

        if (enemiesLeft <= 0)
        {
            FindObjectOfType<WorldManager>().ChangeWorld(20, 0, false, 0);
            whatToDoUponVictory?.Invoke();
        }
    }
}
