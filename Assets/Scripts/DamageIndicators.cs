using System.Collections;
using UnityEngine;

public class DamageIndicators : MonoBehaviour
{
    [SerializeField] GameObject[] damageIndicatorObjects;
    [SerializeField] float timeToDisplayDamageIndicators;

    public GameObject[] DamageIndicatorObjects { get { return damageIndicatorObjects; } set { damageIndicatorObjects = value; } }
    public float TimeToDisplayDamageIndicators { get { return timeToDisplayDamageIndicators; } set { timeToDisplayDamageIndicators = value; } }

    protected IEnumerator ShowDamageIndicatorsRoutine()
    {
        foreach (GameObject indicator in damageIndicatorObjects)
        {
            indicator.SetActive(true);
        }

        yield return new WaitForSeconds(timeToDisplayDamageIndicators);

        foreach (GameObject indicator in damageIndicatorObjects)
        {
            indicator.SetActive(false);
        }
    }
}
