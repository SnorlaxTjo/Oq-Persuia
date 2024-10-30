using UnityEngine;

public class GroundPoundChecker : MonoBehaviour
{
    [SerializeField] float maxSize;
    [SerializeField] float timeToGrow;
    [SerializeField] int damageToDeal;

    float currentSize;
    float currentTime;

    PlayerController playerController;

    public int DamageToDeal { get { return damageToDeal; } }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        currentSize = 1f;
    }

    private void Update()
    {
        currentSize += (maxSize / timeToGrow) * Time.deltaTime;
        currentTime += Time.deltaTime;

        if (currentTime >= timeToGrow)
        {
            playerController.CompleteMoveBlock = false;

            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        transform.localScale = new Vector3(currentSize, 1 , currentSize);
    }
}
