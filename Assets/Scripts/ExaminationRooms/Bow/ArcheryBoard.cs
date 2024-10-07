using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcheryBoard : MonoBehaviour
{
    [SerializeField] HitDisplay hitDisplay;

    bool hasBeenHit;

    BowLevelManager levelManager;

    private void Start()
    {
        levelManager = FindObjectOfType<BowLevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenHit) { return; }

        if (other.gameObject.CompareTag("Arrow"))
        {
            hitDisplay.SetMaterial(1);
            levelManager.TargetsHit++;
            levelManager.CheckAmountOfTargetsHit();

            hasBeenHit = true;
        }
    }
}
