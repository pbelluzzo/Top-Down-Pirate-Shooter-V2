using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class ShipDamageController : MonoBehaviour
    {
        [SerializeField] private ShipSprites shipSprites;
        [SerializeField] private SpriteRenderer shipHull;
        [SerializeField] private SpriteRenderer shipSail;

        [SerializeField] private PoolType destructionEffect;
        [SerializeField] private PoolType damagedEffect;

        private void SetHullSprite(int i) => shipHull.sprite = shipSprites.hulls[i];
        private void SetSailSprite(int i) => shipSail.sprite = shipSprites.sails[i];

        private void RenderDamagedEffect()
        {
            //ObjectPooler.GetInstance().SpawnObjectFromPool(damagedEffect,transform);
        }

        private void ShipController_HealthThresholdChanged(int threshold)
        {
            SetHullSprite(threshold);
            SetSailSprite(threshold);
            if (threshold == 2) RenderDamagedEffect();
        }
    }
}
