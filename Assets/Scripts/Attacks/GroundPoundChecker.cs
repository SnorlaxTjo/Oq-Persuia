using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPoundChecker : MonoBehaviour
{
    [SerializeField] float maxSize;
    [SerializeField] float timeToGrow;

    float currentSize;
    float currentTime;

    PlayerController playerController;

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
            playerController.MoveBlock = false;

            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        transform.localScale = new Vector3(currentSize, 1 , currentSize);
    }
}
