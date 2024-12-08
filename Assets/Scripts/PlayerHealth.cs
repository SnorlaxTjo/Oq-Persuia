using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int[] damageSounds;

    int currentHealth;

    public int MaxHealth { get { return maxHealth; } }
    public int CurrentHealth { get { return currentHealth; } }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;

        int randomSound = Random.Range(0, damageSounds.Length);
        SFXManager.instance.CreateSFX(damageSounds[randomSound]);

        if (currentHealth <= 0)
        {
            Debug.Log("Ded");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BossGroundPound"))
        {
            Damage(other.gameObject.GetComponent<GroundPoundChecker>().DamageToDeal);
        }
    }
}
