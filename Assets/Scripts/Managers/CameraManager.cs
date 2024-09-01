using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector3 lowerLimit;
    [SerializeField] Vector3 upperLimit;

    private void Update()
    {
        float clampedX = Mathf.Clamp(player.position.x, lowerLimit.x, upperLimit.x);
        float clampedZ = Mathf.Clamp(player.position.z, lowerLimit.z, upperLimit.z);    

        Vector3 clampedPos = new Vector3(clampedX + offset.x, player.position.y + offset.y, clampedZ + offset.z);
        transform.position = clampedPos;
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
    }
}
