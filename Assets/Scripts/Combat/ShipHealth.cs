using UnityEngine;

namespace Combat
{
    public abstract class ShipHealth : MonoBehaviour
    {
        [Tooltip("Ship current health")]
        [SerializeField] protected int health = 100;
        [Min(1)]
        [SerializeField] protected int maxHealth = 100;
        [SerializeField] protected GameObject healthBarPrefab;


        protected HealthBar healthBar;
        protected ShipDamageController damageController;

        public int GetMaxHealth() => maxHealth; 

        private void Awake()
        {
            health = maxHealth;
            healthBar = Instantiate(healthBarPrefab,transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<HealthBar>();
            healthBar.SetShipTransform(transform);

            damageController = GetComponent<ShipDamageController>();
        }

        private void Start()
        {
            if (healthBar != null)
                healthBar.SetMaxHealth(maxHealth);

            UpdateHealthBar();
        }

        public bool Damage(float value)
        {
            int roundedValue = Mathf.RoundToInt(value);
            health = Mathf.Clamp(health - roundedValue, 0, maxHealth);
            UpdateHealthBar();

            if (damageController != null)
                damageController.SetDamageLevel(health, maxHealth);

            return CheckDeath();
        }

        protected void UpdateHealthBar()
        {
            if (healthBar != null)
                healthBar.SetHealth(health);
        }

        protected bool CheckDeath()
        {
            if (health <= 0)
            {
                Die();
                return true;
            }
            return false;
        }

        protected abstract void Die();

    }
}

