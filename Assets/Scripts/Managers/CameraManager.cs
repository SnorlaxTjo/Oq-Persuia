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

    bool canCheckForCollision;
    Vector3 standardOffset;
    Vector3 standardRotation;

    public bool CanCheckForCollision { get { return canCheckForCollision; } set { canCheckForCollision = value; } }

    private void Start()
    {
        layerMask = ~layerMask;

        standardOffset = offset;
        standardRotation = cameraTransform.localEulerAngles;
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
        if (!canCheckForCollision) { return; }

        float playerDistance = Mathf.Sqrt(Mathf.Pow(transform.position.z - player.position.z, 2) + Mathf.Pow(transform.position.y - player.position.y, 2));

        RaycastHit hit;
        if (Physics.Raycast(player.position, offset, out hit, playerDistance, layerMask))
        {
            float clampedX = Mathf.Clamp(hit.point.x, lowerLimit.x, upperLimit.x);

            cameraTransform.position = new Vector3(clampedX, hit.point.y, hit.point.z) + hitMoveOffset;
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

    public void ChangeCameraPosition(Vector3 newPosition, float newRotation)
    {
        offset = newPosition;
        cameraTransform.localEulerAngles = new Vector3(newRotation, 0, 0);
    }

    public void ResetCameraPosition()
    {
        offset = standardOffset;
        cameraTransform.localEulerAngles = standardRotation;
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
