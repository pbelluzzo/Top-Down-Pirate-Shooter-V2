using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

namespace Combat
{
    public class AIShipHealth : ShipHealth, IPoolObject
    {
        [Min(0)]
        [SerializeField] int score = 1;
        [SerializeField] PoolType dieEffect;
        [SerializeField] PoolType label;

        ObjectPooler objectPooler;
        public bool givesScore = true;

        public PoolType GetLabel() => label;

        private void Awake()
        {
            healthBar = Instantiate(healthBarPrefab, transform.position, new Quaternion(0, 0, 0, 0)).GetComponent<HealthBar>();
            healthBar.SetShipTransform(transform);

            damageController = GetComponent<ShipDamageController>();
            objectPooler = ObjectPooler.GetInstance();
        }

        protected override void Die()
        {
            GameplayManager gameplayManager = GameplayManager.GetInstance();

            if (givesScore)
                GameplayManager.GetInstance().AddScore(score);

            objectPooler.SpawnFromPool(dieEffect, transform);
            objectPooler.EnqueueObject(label, gameObject);
        }

        private void OnEnable()
        {
            health = maxHealth;
            damageController.SetDamageLevel(health, maxHealth);
            givesScore = true;
            healthBar.gameObject.SetActive(true);
            UpdateHealthBar();
        }

        private void OnDisable()
        {
            if (healthBar != null)
                healthBar.gameObject.SetActive(false);
        }
    }
}

