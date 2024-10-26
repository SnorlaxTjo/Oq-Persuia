using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class FinalBoss : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float smoothTime = 0.1f;
    [SerializeField] Transform playerTransform;
    [SerializeField] BossAttack[] bossAttacks;
    [SerializeField] float timeBetweenEachAttack;
    [SerializeField] Transform head;

    [Header("Attacks")]
    [Space]

    [Header("Ball")]
    [SerializeField] GameObject ball;
    [SerializeField] Transform ballSpawnPoint;

    [Header("Slap")]

    [Header("Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float timeToStayStillBetweenDash;

    [Header("Ground Pound")]
    [SerializeField] float jumpBoost;
    [SerializeField] float jumpTime;
    [SerializeField] float downBoost;
    [SerializeField] GameObject groundPoundObject;
    [SerializeField] Transform groundPoundPosition;
    [SerializeField] float moveBlockTime;

    bool activated;
    float currentVelocity;
    int allAttackProbability;
    float timeSinceLastAttack;
    bool moveBlock;
    float currentMoveSpeed;
    float standardHeight;

    Rigidbody bossRigidbody;

    public bool Activated { get { return activated; } set {  activated = value; } }

    private void Start()
    {
        bossRigidbody = GetComponent<Rigidbody>();

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

    #region Boss Attacks

    public void SpawnBall()
    {
        GameObject summonedBall = Instantiate(ball);
        summonedBall.transform.position = ballSpawnPoint.position;

        Debug.Log("Ball");
    }

    public void Slap()
    {
        Debug.Log("Slap");
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

    IEnumerator DashRoutine()
    {
        moveBlock = true;

        yield return new WaitForSeconds(timeToStayStillBetweenDash);

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
}

[Serializable]
public struct BossAttack
{
    public string name;
    public UnityEvent attack;
    public int probability;
}