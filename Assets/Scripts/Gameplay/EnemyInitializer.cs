using System.Collections;
using UnityEngine;
using Movement;
using Combat;

namespace Control
{
    public class EnemyInitializer : MonoBehaviour
    {
        Collider2D shipCollider;
        ShipController controller;
        ShipMover movement;
        AIShipHealth shipHealth;
        [SerializeField] int initialMovements = 180;

        private void Awake()
        {
            movement = GetComponent<ShipMover>();
            controller = GetComponent<ShipController>();
            shipCollider = GetComponent<Collider2D>();
            shipHealth = GetComponent<AIShipHealth>();
        }
        void OnEnable()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            int i = 0;
            Vector2 direction = transform.position - new Vector3(0, 0, 0);
            transform.up = direction;
            shipCollider.enabled = false;
            controller.enabled = false;

            while (i < initialMovements)
            {
                movement.Move(-1);
                i++;
                yield return new WaitForSeconds(0.01f);
            }

            shipCollider.enabled = true;
            controller.enabled = true;
            this.enabled = false;
            shipHealth.InitializeHealthBar();
        }

    }
}

