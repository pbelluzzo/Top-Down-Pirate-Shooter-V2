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
        void Explode()
        {
            if (shipHealth.GetType() == typeof(AIShipHealth))
                GetComponent<AIShipHealth>().givesScore = false;

            Collider2D[] collidersInsideAoE = Physics2D.OverlapCircleAll(transform.position, areaOfEffect);

            Debug.Log("Colliders in AoE ::");
            foreach (Collider2D collider in collidersInsideAoE)
            {
                Debug.Log(collider.gameObject);
            }

            if (collidersInsideAoE.Length == 0)
                return;

            foreach (Collider2D collider in collidersInsideAoE)
            {
                Debug.Log("Analyzing " + collider.gameObject.name);

                ShipHealth colliderShipHealth = collider.gameObject.GetComponent<ShipHealth>();

                Debug.Log(colliderShipHealth + "   -   " + collider.gameObject.name);

                Debug.Log((colliderShipHealth == null) + " || " + "(" + onlyDamagesPlayer + " && " + !collider.CompareTag("Player") + ")");
                if (colliderShipHealth == null || (onlyDamagesPlayer && !collider.CompareTag("Player")))
                    continue;

                Debug.Log("Damaged " + collider.gameObject.name);

                colliderShipHealth.Damage(explosionDamage);
            }

            DamageSelf();
        }

        private void DamageSelf()
        {
            shipHealth.Damage(shipHealth.GetMaxHealth() + 1);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (onlyExplodesOnPlayer && !collision.gameObject.CompareTag("Player")) 
                return;

            if (collision.gameObject.GetComponent<ShipHealth>() == null)
                return;

            Explode();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, areaOfEffect);
        }

    }
}

