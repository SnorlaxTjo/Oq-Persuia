using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] GameObject arrow;

    [Space]

    [SerializeField] BowStage[] bowStages;
    [SerializeField] float timeBewteenStages;
    [SerializeField] Transform shootPosition;

    int currentStage;
    float timeLeftUntilNextStage;

    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            ShootArrow();
            ResetBow();

            playerController.MoveBlock = false;
        }

        if (Input.GetMouseButton(1))
        {
            if (currentStage < bowStages.Length - 1)
            {
                timeLeftUntilNextStage -= Time.deltaTime;

                if (timeLeftUntilNextStage < 0)
                {
                    timeLeftUntilNextStage += timeBewteenStages;
                    currentStage++;                
                }
            }
            
            foreach (BowStage bowStage in bowStages)
            {
                bowStage.stageObject.SetActive(false);
            }
            bowStages[currentStage].stageObject.SetActive(true);

            playerController.MoveBlock = true;
        }
        else
        {
            ResetBow();
        }       
    }

    void ShootArrow()
    {
        BowStage setBowStage = bowStages[currentStage];

        if (setBowStage.arrowVelocity <= 0) { return; }

        GameObject shotArrow = Instantiate(arrow, shootPosition.position, shootPosition.rotation);

        float velocity = (setBowStage.arrowVelocity);

        shotArrow.GetComponent<Arrow>().SetVelocity(new Vector3(velocity, velocity / 2, velocity));
    }

    void ResetBow()
    {
        currentStage = 0;
        timeLeftUntilNextStage = timeBewteenStages;

        foreach (BowStage bowStage in bowStages)
        {
            bowStage.stageObject.SetActive(false);
        }
    }
}

[Serializable]
public struct BowStage
{
    public GameObject stageObject;
    public float arrowVelocity;
}
