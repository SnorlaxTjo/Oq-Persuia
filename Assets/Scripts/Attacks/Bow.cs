using System;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField] GameObject arrow;

    [Space]

    [SerializeField] BowStage[] bowStages;
    [SerializeField] float timeBewteenStages;
    [SerializeField] Transform shootPosition;
    [SerializeField] int shootSound;

    int currentStage;
    float timeLeftUntilNextStage;
    bool hasObtainedBow = true; // Only temporarily true for playtest
    bool canShoot;

    PlayerController playerController;

    public bool HasObtainedBow { get { return hasObtainedBow; } set { hasObtainedBow = value; } }
    public bool CanShoot { get { return canShoot; } set { canShoot = value; } }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (!hasObtainedBow || !canShoot) { return; }

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
        SFXManager.instance.CreateSFX(shootSound);

        BowStage setBowStage = bowStages[currentStage];

        if (setBowStage.arrowVelocity <= 0) { return; }

        GameObject shotArrow = Instantiate(arrow, shootPosition.position, shootPosition.rotation);

        float velocity = setBowStage.arrowVelocity;
        int damage = setBowStage.damage;

        shotArrow.GetComponent<Arrow>().SetVelocity(new Vector3(velocity, velocity / 2, velocity));
        shotArrow.GetComponent<Arrow>().DamageToDeal = damage;
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
    public int damage;
}
