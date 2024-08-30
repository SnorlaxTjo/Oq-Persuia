using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton : MonoBehaviour
{
    [SerializeField] string id;
    [SerializeField] bool persistent = true;

    public string Id { get { return id; } }

    private void Awake()
    {
        int numberOfObject = 0;
        foreach (PersistentSingleton singleton in FindObjectsOfType<PersistentSingleton>())
        {
            if (singleton.id  == id)
            {
                numberOfObject++;
            }
        }

        if (numberOfObject > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        if (persistent)
        {
            DontDestroyOnLoad(this);
        }
    }
}
