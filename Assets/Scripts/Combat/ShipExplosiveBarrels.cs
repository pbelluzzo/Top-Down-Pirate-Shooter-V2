using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(ShipHealth))]
    public class ShipExplosiveBarrels : ShipWeapon
    {
        [SerializeField] float areaOfEffect;
        [SerializeField] float explosionDamage;
        [Tooltip("Defines if the ship will explode only upon collision with the player or" +
            " if it explodes upon impact")]
        [SerializeField] bool onlyExplodesOnPlayer;
        [Tooltip("Defines if the explosion will damage only the player or any ship inside AoE")]
        [SerializeField] bool onlyDamagesPlayer;
        [SerializeField] PoolType explosionVfx;

        ShipHealth shipHealth;
        private void Awake()
        {
            shipHealth = GetComponent<ShipHealth>();
        }
        void Explode(Collision2D collision)
        {
            if (shipHealth.GetType() == typeof(AIShipHealth))
                GetComponent<AIShipHealth>().givesScore = false;

            collision.gameObject.GetComponent<ShipHealth>().Damage(explosionDamage);

            DamageSelf();
        }

        private void DamageSelf()
        {
            shipHealth.Damage(shipHealth.GetMaxHealth() + 1);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) 
                Explode(collision);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, areaOfEffect);
        }

    }
}

