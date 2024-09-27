using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 hitMoveOffset;
    [SerializeField] Vector3 lowerLimit;
    [SerializeField] Vector3 upperLimit;
    [SerializeField] LayerMask layerMask;

    bool isColliding;

    private void Start()
    {
        layerMask = ~layerMask;
    }

    private void Update()
    {
        float clampedX = Mathf.Clamp(player.position.x, lowerLimit.x, upperLimit.x);
        float clampedZ = Mathf.Clamp(player.position.z, lowerLimit.z, upperLimit.z);    

        Vector3 clampedPos = new Vector3(clampedX + offset.x, player.position.y + offset.y, clampedZ + offset.z);
        transform.position = clampedPos;
    }

    private void FixedUpdate()
    {
        float playerDistance = Mathf.Sqrt(Mathf.Pow(transform.position.z - player.position.z, 2) + Mathf.Pow(transform.position.y - player.position.y, 2));

        RaycastHit hit;
        if (Physics.Raycast(player.position, offset, out hit, playerDistance, layerMask))
        {
            cameraTransform.position = hit.point + hitMoveOffset;
        }
        else
        {
            cameraTransform.position = transform.position;
        }
    }

    public void ChangeCameraLimits(Vector3 newLowerLimit, Vector3 newUpperLimit)
    {
        lowerLimit = newLowerLimit;
        upperLimit = newUpperLimit;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 topLeft = new Vector3 (lowerLimit.x, 0, upperLimit.z);
        Vector3 bottomRight = new Vector3(upperLimit.x, 0, lowerLimit.z);
        Gizmos.DrawLine(lowerLimit, topLeft);
        Gizmos.DrawLine(upperLimit, topLeft);
        Gizmos.DrawLine(lowerLimit, bottomRight);
        Gizmos.DrawLine(upperLimit, bottomRight);
        Gizmos.DrawLine(upperLimit, lowerLimit);
        Gizmos.DrawLine(topLeft, bottomRight);


        Gizmos.color = Color.red;

        Gizmos.DrawLine(player.position, transform.position);
    }
}
