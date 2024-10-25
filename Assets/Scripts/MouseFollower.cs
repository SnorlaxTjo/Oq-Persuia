using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
