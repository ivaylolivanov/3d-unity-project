using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int initialHealthpoints;

    private Slider healthSlider;

    private int healthpoints = 0;
    private int maxHealthpoints = 0;

    private void OnEnable()
    {
        healthpoints = initialHealthpoints;
        maxHealthpoints = initialHealthpoints;

        healthSlider = GetComponentInChildren<Slider>();
        if (healthSlider == null)
        {
            Debug.LogError($"Failed to find slider in Health.cs of {gameObject.name}");
            return;
        }

        healthSlider.maxValue = maxHealthpoints;
        healthSlider.value = healthpoints;
    }

    public void TakeDamage(int damage)
    {
        if(damage <= 0)
            return;

        healthpoints -= damage;

        healthSlider.value = healthpoints;

        if(healthpoints <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if(amount <= 0)
            return;

        healthpoints += amount;
        healthpoints = Mathf.Clamp(healthpoints, 0, maxHealthpoints);

        healthSlider.value = healthpoints;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
