using UnityEngine;
using UnityEngine.InputSystem;
using Combat;
using Movement;

namespace Control
{
    public class ShipController : MonoBehaviour
    {
        ShipMover shipMover;
        [SerializeField] bool haveCannons;
        [SerializeField] ShipCannons shipCannons;
        [SerializeField] bool haveExplosiveBarrels;
        [SerializeField] ShipExplosiveBarrels shipExplosiveBarrels;


        float moveAxisValue = 0f;
        float rotateAxisValue = 0f;
        private void Awake()
        {
            shipMover = GetComponent<ShipMover>();

            if (haveCannons)
                shipCannons = GetComponent<ShipCannons>();

            if (haveExplosiveBarrels)
                shipExplosiveBarrels = GetComponent<ShipExplosiveBarrels>();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            if(moveAxisValue != 0)
            {
                shipMover.Move(moveAxisValue);
            }
            if (rotateAxisValue != 0)
            {
                shipMover.Rotate(rotateAxisValue);
            }
        }

        void OnMove(InputValue inputValue)
        {
            moveAxisValue = inputValue.Get<float>();
        }
        
        void OnRotate(InputValue inputValue)
        {
            rotateAxisValue = inputValue.Get<float>();
        }

        void OnShoot()
        {
            if (!haveCannons || shipCannons == null)
                return;

            shipCannons.ShootFrontCannon();
        }
        void OnShootLeft()
        {
            if (!haveCannons || shipCannons == null)
                return;

            shipCannons.ShootLeftCannons();
        }
        void OnShootRight()
        {
            if (!haveCannons || shipCannons == null)
                return;

            shipCannons.ShootRightCannons();
        }
    }
}

