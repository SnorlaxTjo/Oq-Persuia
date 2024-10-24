using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallParts : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Transform playerTransform;

    bool activated;
    float currentVelocity;

    Rigidbody cannonRigidbody;

    public bool Activated { get { return activated; } set { activated = value; } }

    private void Start()
    {
        cannonRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!activated) { return; }

        Vector3 moveDirection = playerTransform.position - transform.position;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
        moveDirection.Normalize();
        moveDirection *= moveSpeed;

        cannonRigidbody.velocity = moveDirection;

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, 0.1f);

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
