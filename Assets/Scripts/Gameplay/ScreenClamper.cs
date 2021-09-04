using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ScreenClamper : MonoBehaviour
    {
        Camera mainCamera;
        private Vector2 screenBounds;
        [SerializeField] GameObject leftWall;
        [SerializeField] GameObject rightWall;
        [SerializeField] GameObject topWall;
        [SerializeField] GameObject bottomWall;

        void Start()
        {
            mainCamera = Camera.main;
            UpdateWallsPosition();
        }

        private void UpdateWallsPosition()
        {
            Vector3 stageDimensions  = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            leftWall.transform.position = new Vector3(-stageDimensions .x, transform.position.y);
            rightWall.transform.position = new Vector3(stageDimensions .x , transform.position.y);
            topWall.transform.position = new Vector3(transform.position.x, stageDimensions .y);
            bottomWall.transform.position = new Vector3(transform.position.x, -stageDimensions .y);
        }

        private void Update()
        {
            
        }

    }
}
