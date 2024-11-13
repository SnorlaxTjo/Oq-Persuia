using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition + new Vector3(3f, -3f, 0);
    }
}
