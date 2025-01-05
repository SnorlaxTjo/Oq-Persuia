using System.Collections;
using UnityEngine;

public class PlayerHealth : DamageIndicators
{
    [SerializeField] int maxHealth;
    [SerializeField] int[] damageSounds;
    [SerializeField] GameObject deathScreen;
    [SerializeField] float timeBetweenDamageAndRegen;
    [SerializeField] float timeBetweenRegen;

    int currentHealth;
    float timeUntilNextNaturalRegen;

    public int MaxHealth { get { return maxHealth; } }
    public int CurrentHealth { get { return currentHealth; } }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        NaturalRegeneration();
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;

        int randomSound = Random.Range(0, damageSounds.Length);
        SFXManager.instance.CreateSFX(damageSounds[randomSound]);
        timeUntilNextNaturalRegen = timeBetweenDamageAndRegen;

        StartCoroutine(ShowDamageIndicatorsRoutine());

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    void NaturalRegeneration()
    {
        if (currentHealth >= maxHealth) { return; }

        timeUntilNextNaturalRegen -= Time.deltaTime;

        if (timeUntilNextNaturalRegen <= 0)
        {
            currentHealth++;
            timeUntilNextNaturalRegen += timeBetweenRegen;
        }
    }

    void Death()
    {
        FindObjectOfType<PauseManager>().PauseGame(true);
        deathScreen.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BossGroundPound"))
        {
            Damage(other.gameObject.GetComponent<GroundPoundChecker>().DamageToDeal);
        }
    }
}
