using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Control
{
    public class PlayerInputEventsHandler : MonoBehaviour
    {
        InputActionMap inputActionMap;
        InputAction moveAction;
        InputAction rotateAction;
        InputAction shootAction;
        InputAction shootLeftAction;
        InputAction shootRightAction;

        ShipController shipController;

        void Awake()
        {
            inputActionMap = GetComponent<PlayerInput>().currentActionMap;
            shipController = GetComponent<ShipController>();
            RegisterForInputEvents();
        }

        void RegisterForInputEvents()
        {
            moveAction = inputActionMap.FindAction("Move");
            moveAction.performed += shipController.OnMove;
            moveAction.canceled += shipController.OnMove;
            rotateAction = inputActionMap.FindAction("Rotate");
            rotateAction.performed += shipController.OnRotate;
            rotateAction.canceled += shipController.OnRotate;
            shootAction = inputActionMap.FindAction("Shoot");
            shootAction.performed += shipController.OnShoot;
            shootLeftAction = inputActionMap.FindAction("ShootLeft");
            shootLeftAction.performed += shipController.OnShootLeft;
            shootRightAction = inputActionMap.FindAction("ShootRight");
            shootRightAction.performed += shipController.OnShootRight;
        }
    }
}

