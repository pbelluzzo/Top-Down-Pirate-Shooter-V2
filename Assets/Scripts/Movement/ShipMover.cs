using UnityEngine;

namespace Movement
{
    public class ShipMover : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] float rotateSpeed;

        Rigidbody2D shipRigidbody;

        private void Awake()
        {
            shipRigidbody = GetComponent<Rigidbody2D>();
        }

        public void Move()
        {
            shipRigidbody.AddForce(-transform.up * speed * Time.deltaTime);
        }

        public void RotateRight()
        {
            Quaternion pretendedRotation = GetPretendedRotation(-1);

            shipRigidbody.SetRotation(pretendedRotation);
        }

        public void RotateLeft()
        {
            Quaternion pretendedRotation = GetPretendedRotation(1);

            shipRigidbody.SetRotation(pretendedRotation);
        }

        private Quaternion GetPretendedRotation(int directionMultiplier)
        {
            Quaternion currentRotation = transform.rotation;
            float rotateAmount = directionMultiplier * (rotateSpeed * Time.deltaTime);
            Quaternion pretendedRotation = currentRotation;
            pretendedRotation.eulerAngles = currentRotation.eulerAngles += new Vector3(0, 0, rotateAmount);
            return pretendedRotation;
        }

        private void Update() // TODO :: REMOVE
        {
            if (Input.GetKey(KeyCode.W)) Move();
            if (Input.GetKey(KeyCode.D)) RotateRight();
            if (Input.GetKey(KeyCode.A)) RotateLeft();
        }
    }

}
