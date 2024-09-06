using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowLifeTime;

    [SerializeField] Rigidbody arrowRigidbody;

    float lifeTime;
    bool isOnGround;

    Vector3 originalRotation;

    Vector3 previousPosition;
    Vector3 currentPosition;
    Vector3 positionDifference;


    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= arrowLifeTime)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void SetVelocity(Vector3 velocity)
    {
        originalRotation = transform.eulerAngles;
        currentPosition = transform.position;

        Vector3 direction = new Vector3(velocity.x * transform.forward.x, velocity.y, velocity.z * transform.forward.z);

        arrowRigidbody.velocity = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;
    }
}
