using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Minion : DamageIndicators
{
    [SerializeField] int health;
    [SerializeField] float moveSpeed;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] float timeToBeStunned;
    [SerializeField] int damageToPlayer;
    [SerializeField] float hitKnockbackStrength;
    [SerializeField] float distanceToHideArrow;
    [SerializeField] int damageSound;

    [Space]

    [SerializeField] int enemyLayer;
    [SerializeField] int[] layersToIgnoreCollision;

    bool stunned;
    bool knockback;
    int currentHealth;
    float currentVelocity;
    float timeLeftToBeStunned;
    Vector3 movementDirection;
    GameObject correspondingArrow;
    bool forMiniBossfight;

    Transform playerTransform;
    Rigidbody minionRigidbody;
    BossManager bossManager;

    public GameObject CorrespondingArrow { get { return correspondingArrow; } set { correspondingArrow = value; } }
    public bool ForMiniBossfight { get { return forMiniBossfight; } set {  forMiniBossfight = value; } }

    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
        minionRigidbody = GetComponent<Rigidbody>();
        bossManager = FindObjectOfType<BossManager>();

        currentHealth = health;
        timeLeftToBeStunned = timeToBeStunned;

        foreach (int i in layersToIgnoreCollision)
        {
            Physics.IgnoreLayerCollision(enemyLayer, i, true);
        }      
    }

    private void Update()
    {
        // DEBUG ONLY! Remove in final project
        if (Input.GetKeyDown(KeyCode.K))
        {
            Damage(health);
        }

        if (stunned || knockback)
        {
            timeLeftToBeStunned -= Time.deltaTime;
            if (stunned) minionRigidbody.velocity = new Vector3(0, minionRigidbody.velocity.y, 0);

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
    }

    private void FixedUpdate()
    {
        if (stunned || knockback) { return; }

        Vector3 moveDirection = playerTransform.position - transform.position;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
        moveDirection.Normalize();
        moveDirection *= moveSpeed;

        minionRigidbody.velocity = new Vector3(moveDirection.x, minionRigidbody.velocity.y, moveDirection.z);
        movementDirection = moveDirection;

        float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    void Damage(int damage)
    {
        currentHealth -= damage;

        SFXManager.instance.CreateSFX(damageSound);

        StartCoroutine(ShowDamageIndicatorsRoutine());

        if (currentHealth <= 0)
        {
            if (!forMiniBossfight)
            {
                bossManager.RemoveEnemy();
                bossManager.AllEnemyArrows.Remove(correspondingArrow);
            }
            else
            {
                MiniBoss miniBoss = FindObjectOfType<MiniBoss>();
                miniBoss.RemoveEnemy();
                miniBoss.AllEnemyArrows.Remove(correspondingArrow);
            }

            correspondingArrow.SetActive(false);
            Destroy(correspondingArrow);

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
