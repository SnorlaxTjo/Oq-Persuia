using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BossBall : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float lifetime;
    [SerializeField] int damage;

    float timeLived;
    Transform playerTransform;

    Rigidbody ballRigidbody;

    private void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();

        playerTransform = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    private void Update()
    {
        timeLived += Time.deltaTime;

        if (timeLived >= lifetime)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = playerTransform.position - transform.position;
        moveDirection.Normalize();
        moveDirection *= moveSpeed;

        ballRigidbody.velocity = moveDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().Damage(damage);
        }
    }
}
