using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowLifeTime;

    [SerializeField] Rigidbody arrowRigidbody;

    float current;
    float lifeTime;
    bool isOnGround;

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= arrowLifeTime)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        RotateArrow();
    }

    void RotateArrow()
    {
        if (isOnGround) { return; }

        Vector3 rotation = Vector3.RotateTowards(transform.eulerAngles, transform.position + arrowRigidbody.velocity, Time.deltaTime, 0);

        transform.eulerAngles = rotation;
    }

    public void SetVelocity(Vector3 velocity)
    {
        Vector3 direction = new Vector3(velocity.x * transform.forward.x, velocity.y * transform.forward.y, velocity.z * transform.forward.z);

        arrowRigidbody.velocity = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isOnGround = true;
    }
}
