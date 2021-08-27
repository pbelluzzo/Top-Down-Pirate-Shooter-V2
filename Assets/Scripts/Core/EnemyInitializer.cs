using System.Collections;
using UnityEngine;

namespace Core
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
            //ShipController controller = GetComponent<ShipController>();
            //ShipMovement movement = GetComponent<ShipMovement>();
            collider.enabled = false;
            //controller.enabled = false;
            while (i < 64)
            {
                //movement.MoveForward();
                i++;
                yield return new WaitForSeconds(0.01f);
            }
            collider.enabled = true;
            //controller.enabled = true;
            Destroy(this);
        }
    }
}

