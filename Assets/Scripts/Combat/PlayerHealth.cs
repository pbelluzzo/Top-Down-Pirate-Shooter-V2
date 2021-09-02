using UnityEngine;
using Gameplay;

namespace Combat
{
    public class PlayerHealth : ShipHealth
    {
        [SerializeField] PoolType dieEffect;

        protected override void Die()
        {
            ObjectPooler.GetInstance().SpawnFromPool(dieEffect, transform);
            GameplayManager.GetInstance().GameOver();      
        }
    }
}

