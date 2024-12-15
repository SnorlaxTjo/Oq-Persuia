using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProgressKeeper : MonoBehaviour
{
    public static ProgressKeeper instance;

    List<UnityEvent> progressEvents = new List<UnityEvent>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}

[Serializable]
public struct Progress
{
    public string progressId;
    public UnityEvent progressEvent;

    public Progress(string progressId, UnityEvent progressEvent)
    {
        this.progressId = progressId;
        this.progressEvent = progressEvent;
    }
}
