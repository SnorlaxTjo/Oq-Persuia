using UnityEngine;

public class GroundPound : MonoBehaviour
{
    [SerializeField] GameObject groundPoundChecker;
    [SerializeField] Transform groundPoundCheckerSpawnPos;
    [SerializeField] int groundPoundSound;

    PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.IsGrounded) { return; }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playerController.IsGroundPounding = true;
        }
    }

    public void DoGroundPoundAttack()
    {
        GameObject groundPoundObject = Instantiate(groundPoundChecker, groundPoundCheckerSpawnPos);

        playerController.IsGroundPounding = false;
        playerController.CompleteMoveBlock = true;

        SFXManager.instance.CreateSFX(groundPoundSound);
    }
}
