using System.Collections;
using UnityEngine;
using Movement;
using Control;

namespace Gameplay
{
    public class EnemyInitializer : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            int i = 0;
            Collider2D collider = GetComponent<Collider2D>();
            ShipController controller = GetComponent<ShipController>();
            ShipMover movement = GetComponent<ShipMover>();
            collider.enabled = false;
            controller.enabled = false;
            while (i < 160)
            {
                movement.Move(-1);
                i++;
                yield return new WaitForSeconds(0.01f);
            }
            collider.enabled = true;
            controller.enabled = true;
            Destroy(this);
        }
    }
}

