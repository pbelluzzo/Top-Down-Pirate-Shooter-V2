using UnityEngine;
using UnityEngine.InputSystem;
using Combat;
using Movement;
using Gameplay;

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
            if (GameplayManager.GetInstance().GetGameOver())
                Destroy(this);
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

        //void OnMove(InputValue inputValue)
        //{
        //    moveAxisValue = inputValue.Get<float>();
        //}

        public void OnMove(InputAction.CallbackContext obj)
        {
            moveAxisValue = obj.ReadValue<float>();
        }

        public void OnMove(float value = 0f)
        {
            moveAxisValue = value;
        }
        
        //void OnRotate(InputValue inputValue)
        //{
        //    rotateAxisValue = inputValue.Get<float>();
        //}
        public void OnRotate(InputAction.CallbackContext obj)
        {
            rotateAxisValue = obj.ReadValue<float>();
        }
        public void OnRotate(float value)
        {
            rotateAxisValue = value;
        }
        public void OnShoot(InputAction.CallbackContext obj)
        {
            if (!haveCannons || shipCannons == null)
                return;

            shipCannons.ShootFrontCannon();
        }
        public void OnShoot()
        {
            if (!haveCannons || shipCannons == null)
                return;

            shipCannons.ShootFrontCannon();
        }
        //void OnShootLeft()
        //{
        //    if (!haveCannons || shipCannons == null)
        //        return;

        //    shipCannons.ShootLeftCannons();
        //}
        public void OnShootLeft(InputAction.CallbackContext obj)
        {
            if (!haveCannons || shipCannons == null)
                return;

            shipCannons.ShootLeftCannons();
        }
        //void OnShootRight()
        //{
        //    if (!haveCannons || shipCannons == null)
        //        return;

        //    shipCannons.ShootRightCannons();
        //}
        public void OnShootRight(InputAction.CallbackContext obj)
        {
            if (!haveCannons || shipCannons == null)
                return;

            shipCannons.ShootRightCannons();
        }
    }
}

