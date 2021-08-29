using UnityEngine;
using Gameplay;

namespace Combat
{
    public class CannonBall : MonoBehaviour, IPoolObject
    {
        [SerializeField] float damage;
        [SerializeField] float cannonBallSpeed;
        [SerializeField] PoolType explosionEffect;
        [SerializeField] PoolType label;

        ObjectPooler objectPooler;
        Rigidbody2D ballRigidbody;

        public float GetDamage() => damage;
        public PoolType GetLabel() => label;

        private void Awake()
        {
            ballRigidbody = GetComponent<Rigidbody2D>();
            objectPooler = ObjectPooler.GetInstance();
        }

        private void Update()
        {
            Vector3 pretendedPosition = transform.position - transform.up * cannonBallSpeed * Time.deltaTime;
            ballRigidbody.MovePosition(pretendedPosition);
        }
        public void OnCollisionEnter2D(Collision2D collision)
        {
            objectPooler.SpawnFromPool(explosionEffect, transform);
            objectPooler.EnqueueObject(label, gameObject);
        }
    }
}

