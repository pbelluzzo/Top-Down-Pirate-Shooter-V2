using UnityEngine;

namespace Combat
{
    public class CannonBall : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float cannonBallSpeed;
        [SerializeField] private GameObject explosionEffect;

        private Rigidbody2D ballRigidbody;

        public float GetDamage() => damage;

        private void Awake()
        {
            ballRigidbody = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            Vector3 pretendedPosition = transform.position - transform.up * cannonBallSpeed * Time.deltaTime;
            ballRigidbody.MovePosition(pretendedPosition);
        }
        public void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject explosionFx = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(explosionFx, 1f);
            Destroy(this.gameObject);
        }
    }
}

