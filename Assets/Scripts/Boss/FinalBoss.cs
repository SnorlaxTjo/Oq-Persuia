using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FinalBoss : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] Transform playerTransform;

    bool activated;
    float currentVelocity;

    Rigidbody bossRigidbody;

    public bool Activated { get { return activated; } set {  activated = value; } }

    private void Start()
    {
        bossRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!activated) { return; }

        Vector3 moveDirection = playerTransform.position - transform.position;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
        moveDirection.Normalize();
        moveDirection *= moveSpeed;

        bossRigidbody.velocity = new Vector3(moveDirection.x, bossRigidbody.velocity.y, moveDirection.z);

        float targetAngle = Mathf.Atan2(-moveDirection.x, -moveDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
}
