using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldInfo : MonoBehaviour
{
    [SerializeField] Vector3 lowerCameraLimit;
    [SerializeField] Vector3 upperCameraLimit;
    [SerializeField] float worldHeight;

    [Space]

    [Header("Custom Camera Position")]
    [SerializeField] bool useCustomCameraPosition;
    [SerializeField] Vector3 cameraPlayerOffset;
    [SerializeField] float cameraRotation;

    Vector3 finalLowerCameraLimit;
    Vector3 finalUpperCameraLimit;

    public Vector3 LowerCameraLimit { get { return finalLowerCameraLimit; } }
    public Vector3 UpperCameraLimit { get { return finalUpperCameraLimit; } }

    public bool UseCustomCameraPosition { get {  return useCustomCameraPosition; } }
    public Vector3 CameraPlayerOffset { get {  return cameraPlayerOffset; } }
    public float CameraRotation { get { return cameraRotation; } }

    private void Start()
    {
        finalLowerCameraLimit = transform.position + lowerCameraLimit;
        finalUpperCameraLimit = transform.position + upperCameraLimit;

        if (lowerCameraLimit.x > upperCameraLimit.x)
        {
            Debug.LogWarning("Lower Camera X is higher than Upper Camera X on " + gameObject.name + ". This will cause issues if not fixed");
        }
        if (lowerCameraLimit.z > upperCameraLimit.z)
        {          
            Debug.LogWarning("Lower Camera Z is higher than Upper Camera Z on " + gameObject.name + ". This will cause issues if not fixed");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 topLeft = new Vector3(transform.position.x + lowerCameraLimit.x, worldHeight, transform.position.z + upperCameraLimit.z);
        Vector3 bottomRight = new Vector3(transform.position.x + upperCameraLimit.x, worldHeight, transform.position.z + lowerCameraLimit.z);
        Gizmos.DrawLine(transform.position + lowerCameraLimit, topLeft);
        Gizmos.DrawLine(transform.position + upperCameraLimit, topLeft);
        Gizmos.DrawLine(transform.position + lowerCameraLimit, bottomRight);
        Gizmos.DrawLine(transform.position + upperCameraLimit, bottomRight);
        Gizmos.DrawLine(transform.position + upperCameraLimit, transform.position + lowerCameraLimit);
        Gizmos.DrawLine(topLeft, bottomRight);
    }
}
