using UnityEngine;
using UnityEngine.Events;

public class TriggerOnTouch : MonoBehaviour
{
    [SerializeField] UnityEvent whatToDoUponTouch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            whatToDoUponTouch?.Invoke();
        }
    }
}
