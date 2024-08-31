using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    private void Update()
    {
        transform.position = Input.mousePosition;
    }
}
