using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;

namespace Combat
{
    public class ShipDamageController : MonoBehaviour
    {
        [SerializeField] private ShipSpriteGroup shipSprites;
        [SerializeField] private SpriteRenderer shipHull;
        [SerializeField] private SpriteRenderer shipSail;

        [SerializeField] private PoolType destructionEffect;
        [SerializeField] private PoolType damagedEffect;

        GameObject damagedFxObject;
        public void SetDamageLevel(float health, float maxHealth)
        {
            float threshold = health / maxHealth;

            if (threshold >= 0.7)
            {
                SetHullSprite(0);
                SetSailSprite(0);
                if (damagedFxObject != null)
                    damagedFxObject.GetComponent<PoolVfx>().ReturnToQueueImmediately();
                return;
            }
            if (threshold < 0.7 && threshold > 0.3)
            {
                SetHullSprite(1);
                SetSailSprite(1);
                if (damagedFxObject != null)
                    damagedFxObject.GetComponent<PoolVfx>().ReturnToQueueImmediately();
                return;
            }
            if (threshold <= 0.3)
            {
                SetHullSprite(2);
                SetSailSprite(2);
                RenderDamagedEffect();
                return;
            }
        }

        private void SetHullSprite(int i) => shipHull.sprite = shipSprites.hulls[i];
        private void SetSailSprite(int i) => shipSail.sprite = shipSprites.sails[i];

        private void RenderDamagedEffect()
        {
            if (damagedFxObject == null)
                damagedFxObject = ObjectPooler.GetInstance().SpawnFromPool(damagedEffect, transform, true);
        }
    }
}
