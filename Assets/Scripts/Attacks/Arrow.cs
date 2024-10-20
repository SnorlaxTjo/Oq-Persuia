using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowLifeTime;
    [SerializeField] float raycastLength;
    [SerializeField] GameObject hitPoint;

    [SerializeField] Rigidbody arrowRigidbody;

    float lifeTime;
    bool hasHit;
    int damageToDeal;

    public int DamageToDeal { get { return damageToDeal; } set {  damageToDeal = value; } }


    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= arrowLifeTime)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (hasHit) { return; }

        Ray arrowRay = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(arrowRay, out hit, raycastLength))
        {
            if (hit.collider.gameObject.CompareTag("ArrowHitPoint"))
            {
                hasHit = true;

                GameObject arrowHitPoint = Instantiate(hitPoint);
                arrowHitPoint.transform.position = hit.point;
            }
        }
    }

    public void SetVelocity(Vector3 velocity)
    { 
        Vector3 direction = new Vector3(velocity.x * transform.forward.x, velocity.y, velocity.z * transform.forward.z);

        arrowRigidbody.velocity = direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (transform.forward * raycastLength));
    }
}
