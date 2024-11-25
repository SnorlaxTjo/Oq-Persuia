using UnityEngine;

public class WorldInfo : MonoBehaviour
{
    [SerializeField] bool alwaysShowCameraGizmos;

    [Space]

    [SerializeField] Vector3 lowerCameraLimit;
    [SerializeField] Vector3 upperCameraLimit;
    [SerializeField] float worldHeight;

    [Space]
    [SerializeField] bool turnOffCameraCollision;

    [Space]
    [Header("Custom Camera Position")]
    [SerializeField] bool useCustomCameraPosition;
    [SerializeField] Vector3 cameraPlayerOffset;
    [SerializeField] float cameraRotation;

    [Space]
    [Header("Active Weapons in World")]
    [SerializeField] bool bow;
    [SerializeField] bool groundPound;

    [Space]
    [SerializeField] bool canFastTravel = true;

    Vector3 finalLowerCameraLimit;
    Vector3 finalUpperCameraLimit;

    public Vector3 LowerCameraLimit { get { return finalLowerCameraLimit; } }
    public Vector3 UpperCameraLimit { get { return finalUpperCameraLimit; } }

    public bool TurnOffCameraCollisions { get { return turnOffCameraCollision; } }

    public bool UseCustomCameraPosition { get {  return useCustomCameraPosition; } set { useCustomCameraPosition = value; } }
    public Vector3 CameraPlayerOffset { get {  return cameraPlayerOffset; } }
    public float CameraRotation { get { return cameraRotation; } }

    public bool Bow { get { return bow; } set { bow = value; } }
    public bool GroundPound { get { return groundPound; } }

    public bool CanFastTravel { get {  return canFastTravel; } }

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



    private void OnDrawGizmos()
    {
        if (!alwaysShowCameraGizmos) { return; }
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

    private void OnDrawGizmosSelected()
    {
        if (alwaysShowCameraGizmos) { return; }
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
