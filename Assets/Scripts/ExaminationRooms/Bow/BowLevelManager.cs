using System;
using UnityEngine;
using UnityEngine.Events;

public class BowLevelManager : MonoBehaviour
{
    [SerializeField] int worldToTeleportOutTo;
    [SerializeField] int teleporter;

    [SerializeField] Level[] levels;
    [SerializeField] GameObject movingFloor;
    [SerializeField] float distanceToMove;
    [SerializeField] float timeToMove;
    [SerializeField] float timeToWait;

    [Space]
    [SerializeField] UnityEvent whatMoreToDoUponCompletion;

    int currentLevel;
    GameObject currentLevelObject;
    int targetsHit;
    bool isMoving;
    int moveStage;
    float moveTimePassed;

    float originalFloorPosition;

    WorldManager worldManager;

    public int TargetsHit { get { return targetsHit; } set {  targetsHit = value; } }

    private void Start()
    {
        worldManager = FindObjectOfType<WorldManager>();

        currentLevel = 0;

        foreach(Level level in levels)
        {
            level.levelObject.SetActive(false);
        }
        levels[0].levelObject.SetActive(true);

        currentLevelObject = levels[0].levelObject;

        originalFloorPosition = movingFloor.transform.localPosition.y;
    }

    private void Update()
    {
        if (!isMoving) { return; }

        switch(moveStage)
        {
            case 0:
                float distanceToMoveNow = (distanceToMove / timeToMove) * Time.deltaTime;
                movingFloor.transform.localPosition += new Vector3(0, distanceToMoveNow, 0);
                currentLevelObject.transform.localPosition += new Vector3(0, distanceToMoveNow, 0);
                moveTimePassed += Time.deltaTime;

                if (moveTimePassed >= timeToMove)
                {
                    moveTimePassed -= timeToMove;
                    movingFloor.transform.localPosition = new Vector3(movingFloor.transform.localPosition.x, originalFloorPosition + distanceToMove, movingFloor.transform.localPosition.z);
                    currentLevelObject.transform.localPosition = new Vector3(0, distanceToMove, 0);

                    currentLevel++;
                    targetsHit = 0;
                    moveStage = 1;
                }
                break;

            case 1:
                for (int i = 0; i < levels.Length; i++)
                {
                    levels[i].levelObject.SetActive(i == currentLevel);
                }
                currentLevelObject = levels[currentLevel].levelObject;
                moveTimePassed += Time.deltaTime;

                if (moveTimePassed >= timeToWait)
                {
                    moveTimePassed -= timeToWait;
                    moveStage = 2;
                }
                break;

            case 2:
                float distanceToMoveUpNow = (-distanceToMove / timeToMove) * Time.deltaTime;
                movingFloor.transform.localPosition += new Vector3(0, distanceToMoveUpNow, 0);
                currentLevelObject.transform.localPosition += new Vector3(0, distanceToMoveUpNow, 0);
                moveTimePassed += Time.deltaTime;

                if (moveTimePassed >= timeToMove)
                {
                    moveTimePassed -= timeToMove;
                    movingFloor.transform.localPosition = new Vector3(movingFloor.transform.localPosition.x, originalFloorPosition, movingFloor.transform.localPosition.z);
                    currentLevelObject.transform.localPosition = new Vector3(0, 0, 0);

                    moveStage = 0;
                    isMoving = false;
                }
                break;

            default:
                Debug.LogWarning("Move Stage is not a valid number, please check");
                break;
        }
    }

    public void CheckAmountOfTargetsHit()
    {
        if (targetsHit >= levels[currentLevel].targetAmount)
        {
            if (currentLevel >= levels.Length - 1)
            {
                worldManager.ChangeWorld(worldToTeleportOutTo, teleporter, false, 0);
                whatMoreToDoUponCompletion?.Invoke();
                return;
            }

            ArrowHitPoint[] allArrowHitPoints = FindObjectsOfType<ArrowHitPoint>();
            foreach(ArrowHitPoint arrowHitPoint in allArrowHitPoints)
            {
                arrowHitPoint.gameObject.SetActive(false);
                Destroy(arrowHitPoint.gameObject);
            }

            isMoving = true;
        }
    }
}

[Serializable]
public struct Level
{
    public GameObject levelObject;
    public int targetAmount;
}
