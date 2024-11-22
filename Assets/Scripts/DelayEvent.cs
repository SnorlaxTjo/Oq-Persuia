using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DelayEvent : MonoBehaviour
{
    [SerializeField] UnityEvent eventToDo;
    [SerializeField] float timeToDelay;

    public void StartDelay()
    {
        StartCoroutine(DelayEventRoutine());
    }

    IEnumerator DelayEventRoutine()
    {
        yield return new WaitForSeconds(timeToDelay);

        eventToDo?.Invoke();
    }
}
