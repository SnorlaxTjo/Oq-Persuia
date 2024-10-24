using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallParts : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform playerTransform;
    [SerializeField] float distanceToHideArrow;
    [SerializeField] float distanceToFreezeFromPlayer;
    [SerializeField] int damageToPlayer;

    [Header("Shooting")]
    [SerializeField] Transform eye;
    [SerializeField] GameObject enemyProjectile;
    [SerializeField] float timeBetweenEachProjectile;

    [Header("Only Required for the Right One")]
    [SerializeField] bool isRightOne;
    [SerializeField] Transform leftCannon;

    float currentHealth;
    bool activated;
    float currentVelocity;
    GameObject correspondingArrow;
    float timeSinceLastProjectile;

    Rigidbody cannonRigidbody;
    BossManager bossManager;

    public bool Activated { get { return activated; } set { activated = value; } }
    public GameObject CorrespondingArrow { get { return correspondingArrow; } set { correspondingArrow = value; } }

    private void Start()
    {
        cannonRigidbody = GetComponent<Rigidbody>();
        bossManager = FindObjectOfType<BossManager>();

        currentHealth = health;
    }

    private void FixedUpdate()
    {
        if (!activated) { return; }

        if (!isRightOne)
        {
            Vector3 moveDirection = playerTransform.position - transform.position;
            moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
            moveDirection.Normalize();
            moveDirection *= moveSpeed;

            if (Mathf.Abs(Vector3.Distance(transform.position, playerTransform.position)) > distanceToFreezeFromPlayer)
            {               
                cannonRigidbody.velocity = moveDirection;
            }
            else
            {
                cannonRigidbody.velocity = Vector3.zero;
            }            

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, 0.1f);

            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        else
        {
            if (leftCannon == null)
            {
                isRightOne = false;
                return;
            }

            transform.localPosition = new Vector3(-leftCannon.localPosition.x, leftCannon.localPosition.y, leftCannon.localPosition.z);
            transform.localEulerAngles = new Vector3(leftCannon.localEulerAngles.x, -leftCannon.localEulerAngles.y, leftCannon.localEulerAngles.z);
        }
    }

    private void Update()
    {
        if (!activated) { return; }

        Vector3 enemyDirection = transform.position - correspondingArrow.transform.position;
        Vector3 newRotation = Vector3.RotateTowards(correspondingArrow.transform.forward, enemyDirection, Time.deltaTime, 0);

        correspondingArrow.transform.rotation = Quaternion.LookRotation(newRotation);
        correspondingArrow.transform.eulerAngles = new Vector3(0, correspondingArrow.transform.eulerAngles.y, 0);

        float playerDistance = Vector3.Distance(playerTransform.position, transform.position);

        if (Mathf.Abs(playerDistance) <= distanceToHideArrow)
        {
            correspondingArrow.SetActive(false);
        }
        else
        {
            correspondingArrow.SetActive(true);
        }

        Vector3 targetDirection = playerTransform.position - eye.transform.position;
        Vector3 newDirection = Vector3.RotateTowards(eye.transform.forward, targetDirection, Time.deltaTime, 0);
        eye.transform.rotation = Quaternion.LookRotation(newDirection);

        timeSinceLastProjectile += Time.deltaTime;

        if (timeSinceLastProjectile >= timeBetweenEachProjectile)
        {
            Instantiate(enemyProjectile, eye.transform.position, eye.transform.rotation);

            timeSinceLastProjectile -= timeBetweenEachProjectile;
        }
    }

    void Damage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            bossManager.RemoveEnemy();
            bossManager.AllEnemyArrows.Remove(correspondingArrow);

            correspondingArrow.SetActive(false);
            Destroy(correspondingArrow);

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated) { return; }

        if (other.gameObject.CompareTag("Arrow"))
        {
            Damage(other.gameObject.GetComponent<Arrow>().DamageToDeal);

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().Damage(damageToPlayer);
        }
    }
}
