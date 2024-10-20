using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Minion : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float moveSpeed;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float timeToBeStunned;
    [SerializeField] int damageToPlayer;
    [SerializeField] float hitKnockbackStrength;

    bool stunned;
    bool knockback;
    int currentHealth;
    float currentVelocity;
    float timeLeftToBeStunned;
    Vector3 movementDirection;

    Transform playerTransform;
    Rigidbody minionRigidbody;
    BossManager bossManager;

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
        minionRigidbody = GetComponent<Rigidbody>();
        bossManager = FindObjectOfType<BossManager>();

        currentHealth = health;
        timeLeftToBeStunned = timeToBeStunned;
    }

    private void Update()
    {
        if (stunned || knockback)
        {
            timeLeftToBeStunned -= Time.deltaTime;
            if (stunned) minionRigidbody.velocity = Vector3.zero;

            if (timeLeftToBeStunned <= 0)
            {
                stunned = false;
                knockback = false;
            }
        }
        else
        {
            timeLeftToBeStunned = timeToBeStunned;
        }
    }

    private void FixedUpdate()
    {
        if (stunned || knockback) { return; }

        Vector3 moveDirection = playerTransform.position - transform.position;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
        moveDirection.Normalize();

        minionRigidbody.velocity = moveDirection * moveSpeed;
        movementDirection = moveDirection;

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    void Damage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            bossManager.RemoveEnemy();

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            Damage(other.gameObject.GetComponent<Arrow>().DamageToDeal);
            stunned = true;

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().Damage(damageToPlayer);
            knockback = true;

            Vector3 knockbackVelocity = hitKnockbackStrength * -movementDirection;
            minionRigidbody.velocity = new Vector3(knockbackVelocity.x, hitKnockbackStrength / 2, knockbackVelocity.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GroundPound"))
        {
            Damage(other.gameObject.GetComponent<GroundPoundChecker>().DamageToDeal);
            stunned = true;
        }
    }
}
