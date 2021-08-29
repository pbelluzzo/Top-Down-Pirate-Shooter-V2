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

        public void Move(float axisValue)
        {
            shipRigidbody.AddForce(axisValue * (transform.up * speed * Time.deltaTime));
        }

        public void Rotate(float axisValue)
        {
            Quaternion pretendedRotation = GetPretendedRotation(axisValue);

            shipRigidbody.SetRotation(pretendedRotation);
        }

        public void RotateLeft()
        {
            Quaternion pretendedRotation = GetPretendedRotation(1);

            shipRigidbody.SetRotation(pretendedRotation);
        }

        private Quaternion GetPretendedRotation(float directionMultiplier)
        {
            Quaternion currentRotation = transform.rotation;
            float rotateAmount = directionMultiplier * (rotateSpeed * Time.deltaTime);
            Quaternion pretendedRotation = currentRotation;
            pretendedRotation.eulerAngles = currentRotation.eulerAngles += new Vector3(0, 0, rotateAmount);
            return pretendedRotation;
        }
    }

}
