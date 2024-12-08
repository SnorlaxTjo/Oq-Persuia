using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class FinalBoss : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int damageSound;

    [Space]

    [SerializeField] float moveSpeed;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] Transform playerTransform;
    [SerializeField] BossAttack[] bossAttacks;
    [SerializeField] float timeBetweenEachAttack;
    [SerializeField] Transform head;
    [SerializeField] int damageToPlayer;
    [SerializeField] float stunTime;

    [Header("Attacks")]
    [Space]

    [Header("Ball")]
    [SerializeField] GameObject ball;
    [SerializeField] Transform ballSpawnPoint;
    [SerializeField] int ballSound;

    [Header("Slap")]
    [SerializeField] float animationTime;
    [SerializeField] int slapSound;

    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float timeToStayStillBetweenDash;
    [SerializeField] int dashSound;

    [Header("Ground Pound")]
    [SerializeField] float jumpBoost;
    [SerializeField] float jumpTime;
    [SerializeField] float downBoost;
    [SerializeField] GameObject groundPoundObject;
    [SerializeField] Transform groundPoundPosition;
    [SerializeField] float moveBlockTime;
    [SerializeField] int groundPoundSound;

    [Space]
    [Space]

    [SerializeField] UnityEvent whatToDoUponVictory;

    int currentHealth;
    bool activated;
    float currentVelocity;
    int allAttackProbability;
    float timeSinceLastAttack;
    bool moveBlock;
    float currentMoveSpeed;
    float standardHeight;
    bool damageBlock;
    Vector3 originalPosition;

    Rigidbody bossRigidbody;
    Animator bossAnimator;
    UIManager uiManager;

    public bool Activated { get { return activated; } set {  activated = value; } }

    private void Start()
    {
        bossRigidbody = GetComponent<Rigidbody>();
        bossAnimator = GetComponent<Animator>();
        uiManager = FindObjectOfType<UIManager>();

        foreach(BossAttack bossAttack in bossAttacks)
        {
            if (bossAttack.probability <= 0)
            {
                Debug.LogWarning("A boss attack has 0 probability, please check.");
                continue;
            }
            allAttackProbability += bossAttack.probability;
        }

        currentMoveSpeed = moveSpeed;
        standardHeight = transform.position.y;
        currentHealth = health;

        originalPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (!activated) { return; }

        if (moveBlock)
        {
            bossRigidbody.velocity = new Vector3(0, bossRigidbody.velocity.y, 0);
            return;
        }

        Vector3 moveDirection = playerTransform.position - transform.position;
        moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z);
        moveDirection.Normalize();
        moveDirection *= currentMoveSpeed;

        bossRigidbody.velocity = new Vector3(moveDirection.x, bossRigidbody.velocity.y, moveDirection.z);

        float targetAngle = Mathf.Atan2(-moveDirection.x, -moveDirection.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Update()
    {
        if (!activated) { return; }

        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= timeBetweenEachAttack)
        {
            PerformRandomAttack();
            timeSinceLastAttack -= timeBetweenEachAttack;
        }

        Vector3 targetDirection = head.transform.position - playerTransform.position;
        Vector3 newDirection = Vector3.RotateTowards(head.transform.forward, targetDirection, Time.deltaTime, 0);
        head.transform.rotation = Quaternion.LookRotation(newDirection);

        if (transform.position.y < standardHeight)
        {
            bossRigidbody.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, standardHeight, transform.position.z);

            GameObject summonedGroundPound = Instantiate(groundPoundObject, groundPoundPosition);

            SFXManager.instance.CreateSFX(groundPoundSound);

            StartCoroutine(MoveBlockRoutine());
        }
    }

    void PerformRandomAttack()
    {
        int attackToDo = UnityEngine.Random.Range(0, allAttackProbability);
        int attackCheck = -1;
        bool hasAttacked = false;

        for (int i = 0; hasAttacked == false; i++)
        {
            attackCheck += bossAttacks[i].probability;
            if (attackCheck >= attackToDo)
            {
                bossAttacks[i].attack?.Invoke();
                hasAttacked = true;
            }
        }
    }

    void Damage(int damage)
    {
        if (damageBlock) { return; }

        currentHealth -= damage;

        SFXManager.instance.CreateSFX(damageSound);

        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        uiManager.SetBossHealthBar(currentHealth, health);

        if (currentHealth <= 0)
        {
            activated = false;
            bossRigidbody.velocity = Vector3.zero;
            FindObjectOfType<WorldManager>().ChangeWorld(24, 0, true, 4);

            whatToDoUponVictory?.Invoke();
        }
    }

    #region Boss Attacks

    public void SpawnBall()
    {
        SFXManager.instance.CreateSFX(ballSound);

        GameObject summonedBall = Instantiate(ball);
        summonedBall.transform.position = ballSpawnPoint.position;

        Debug.Log("Ball");
    }

    public void Slap()
    {
        StartCoroutine(SlapRoutine());

        Debug.Log("Slap");
    }

    public void PlaySlapSound()
    {
        SFXManager.instance.CreateSFX(slapSound);
    }

    public void Dash()
    {
        StartCoroutine(DashRoutine());

        Debug.Log("Dash");
    }

    public void GroundPound()
    {       
        StartCoroutine(GroundPoundRoutine());

        Debug.Log("Ground Pound");
    }

    IEnumerator SlapRoutine()
    {
        moveBlock = true;
        bossAnimator.SetBool("SlapAttack", true);

        yield return new WaitForSeconds(animationTime);

        moveBlock = false;
        bossAnimator.SetBool("SlapAttack", false);
    }

    IEnumerator DashRoutine()
    {
        moveBlock = true;

        yield return new WaitForSeconds(timeToStayStillBetweenDash);

        SFXManager.instance.CreateSFX(dashSound);
        currentMoveSpeed = dashSpeed;
        moveBlock = false;

        yield return new WaitForSeconds(dashTime);

        moveBlock = true;

        yield return new WaitForSeconds(timeToStayStillBetweenDash);

        currentMoveSpeed = moveSpeed;
        moveBlock = false;
    }

    IEnumerator GroundPoundRoutine()
    {
        bossRigidbody.velocity += new Vector3(0, jumpBoost, 0);

        yield return new WaitForSeconds(jumpTime);

        bossRigidbody.velocity = new Vector3(bossRigidbody.velocity.x, 0, bossRigidbody.velocity.z);

        yield return new WaitForSeconds(0.1f);

        bossRigidbody.velocity += new Vector3(0, -downBoost, 0);
    }

    IEnumerator MoveBlockRoutine()
    {
        moveBlock = true;

        yield return new WaitForSeconds(moveBlockTime);

        moveBlock = false;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().Damage(damageToPlayer);
        }
        else if (other.gameObject.CompareTag("Arrow"))
        {
            Damage(other.GetComponent<Arrow>().DamageToDeal);

            other.gameObject.SetActive(false);
            Destroy(other.gameObject);

            StartCoroutine(DamageBlockRoutine());
        }
        else if (other.gameObject.CompareTag("GroundPound"))
        {
            Damage(other.gameObject.GetComponent<GroundPoundChecker>().DamageToDeal);

            StartCoroutine(DamageBlockRoutine());
        }
    }

    IEnumerator DamageBlockRoutine()
    {
        damageBlock = true;

        yield return new WaitForSeconds(1);

        damageBlock = false;
    }

    public void ResetPosition()
    {
        bossRigidbody.position = originalPosition;
        bossRigidbody.rotation = Quaternion.identity;

        transform.position = originalPosition;
        transform.localEulerAngles = Vector3.zero;
    }
}

[Serializable]
public struct BossAttack
{
    public string name;
    public UnityEvent attack;
    public int probability;
}