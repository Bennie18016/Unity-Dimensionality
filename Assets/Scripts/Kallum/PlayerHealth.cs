using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public HealthBar healthBar;

    void Start()
    {

        currentHealth = maxHealth;
        healthBar = GameObject.FindGameObjectWithTag("health bar").GetComponent<HealthBar>();

        healthBar.SetMaxHealth(maxHealth);

    }

    // Update is called once per frame

    private void checkHealth()
    {
        if(currentHealth <= 0)
        {
            gameObject.transform.position = new Vector3(1.91f, -2.91f, -5.37f);
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth); 
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        checkHealth();
    }

    public void MaxHealth(){
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);
    }
}
