using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class ShipHealth : MonoBehaviour
    {
        [Tooltip("Ship current health")]
        [SerializeField] private int health = 100;
        [Min(1)]
        [SerializeField] private int maxHealth = 100;

        [SerializeField] GameObject healthBarPrefab;

        HealthBar healthBar;
        bool isDead = false;

        public int GetMaxHealth() => maxHealth; 

        private void Awake()
        {
            healthBar = Instantiate(healthBarPrefab,transform.position,transform.rotation).GetComponent<HealthBar>();
            healthBar.SetShipTransform(transform);

            if (healthBar != null)
                healthBar.SetMaxHealth(maxHealth);

            UpdateHealthBar();

            //TODO :: Pooling
        }

        public bool Damage(float value)
        {
            int roundedValue = Mathf.RoundToInt(value);
            health = Mathf.Clamp(health - roundedValue, 0, maxHealth);
            UpdateHealthBar();
            return CheckDeath();
        }

        private void UpdateHealthBar()
        {
            if (healthBar != null)
                healthBar.SetHealth(health);
        }

        private bool CheckDeath()
        {
            if (health <= 0)
            {
                Die();
                return true;
            }
            return false;
        }

        public void Die()
        {
            if (isDead)
                return;

            isDead = true;
        }
    }
}

