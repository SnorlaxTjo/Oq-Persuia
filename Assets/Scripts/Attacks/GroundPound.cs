using UnityEngine;

public class GroundPound : MonoBehaviour
{
    [SerializeField] GameObject groundPoundChecker;
    [SerializeField] Transform groundPoundCheckerSpawnPos;

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
        playerController.MoveBlock = true;
    }
}
