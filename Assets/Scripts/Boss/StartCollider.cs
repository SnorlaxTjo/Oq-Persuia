using UnityEngine;

public class StartCollider : MonoBehaviour
{
    BossManager bossManager;

    private void Start()
    {
        bossManager = FindObjectOfType<BossManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bossManager.SummonNextWave();

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
