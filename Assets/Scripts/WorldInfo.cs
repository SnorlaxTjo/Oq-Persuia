using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldInfo : MonoBehaviour
{
    [SerializeField] Vector3 lowerCameraLimit;
    [SerializeField] Vector3 upperCameraLimit;
    [SerializeField] float worldHeight;

    public Vector3 LowerCameraLimit { get { return lowerCameraLimit; } }
    public Vector3 UpperCameraLimit { get { return upperCameraLimit; } }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 topLeft = new Vector3(lowerCameraLimit.x, worldHeight, upperCameraLimit.z);
        Vector3 bottomRight = new Vector3(upperCameraLimit.x, worldHeight, lowerCameraLimit.z);
        Gizmos.DrawLine(lowerCameraLimit, topLeft);
        Gizmos.DrawLine(upperCameraLimit, topLeft);
        Gizmos.DrawLine(lowerCameraLimit, bottomRight);
        Gizmos.DrawLine(upperCameraLimit, bottomRight);
        Gizmos.DrawLine(upperCameraLimit, lowerCameraLimit);
        Gizmos.DrawLine(topLeft, bottomRight);
    }
}
