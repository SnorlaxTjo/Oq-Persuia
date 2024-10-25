using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth;

    int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Ded");
        }
    }
}
