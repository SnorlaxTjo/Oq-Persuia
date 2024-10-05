using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitPoint : MonoBehaviour
{
    [SerializeField] float lifeTime;

    float lifeTimeLeft;

    private void Start()
    {
        lifeTimeLeft = lifeTime;
    }

    private void Update()
    {
        lifeTimeLeft -= Time.deltaTime;
        if (lifeTimeLeft <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
