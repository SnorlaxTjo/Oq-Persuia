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
        Debug.Log("a");
        StartCoroutine(DelayEventRoutine());
    }

    IEnumerator DelayEventRoutine()
    {
        yield return new WaitForSeconds(timeToDelay);

        eventToDo?.Invoke();
    }
}
