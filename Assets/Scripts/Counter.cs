using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Counter : MonoBehaviour
{
    [SerializeField] int countGoal;
    [SerializeField] UnityEvent whatToDoWhenGoalReached;

    int currentNumber;

    public void AddNumber(int numberToAdd)
    {
        currentNumber += numberToAdd;

        if (currentNumber >= countGoal)
        {
            whatToDoWhenGoalReached?.Invoke();
        }
    }
}
