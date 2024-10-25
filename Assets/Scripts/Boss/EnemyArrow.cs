using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyArrow : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float lifetime;
    [SerializeField] int damage;

    float timeExisted;

    Rigidbody arrowRigidbody;

    private void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody>();

        arrowRigidbody.velocity = transform.forward.normalized * moveSpeed;
    }

    private void Update()
    {
        timeExisted += Time.deltaTime;

        if (timeExisted >= lifetime)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().Damage(damage);           
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
