using UnityEngine;
using Gameplay;

namespace Combat
{
    public class ShipCannons : ShipWeapon
    {
        [Tooltip("Projectile type from the object pooler")]
        [SerializeField] PoolType projectile;
        [SerializeField] float cannonsCooldown;

        [Header("Define cannonball spawn transforms")]
        [SerializeField] Transform frontCannon;
        [SerializeField] Transform[] leftCannons;
        [SerializeField] Transform[] rightCannons;

        float frontalCannonCooldown = 0;
        float leftCannonsCooldown = 0;
        float rightCannonsCooldown = 0;

        private void Update()
        {
            SetCooldowns();
        }

        void SetCooldowns()
        {
            frontalCannonCooldown += Time.deltaTime;
            leftCannonsCooldown += Time.deltaTime;
            rightCannonsCooldown += Time.deltaTime;
        }

        public void ShootFrontCannon() 
        {
            if (frontalCannonCooldown < cannonsCooldown)
                return;

            ObjectPooler.GetInstance().SpawnFromPool(projectile,frontCannon);
            frontalCannonCooldown = 0;
        }

        public void ShootLeftCannons()
        {
            if (leftCannonsCooldown < cannonsCooldown)
                return;

            foreach(Transform leftCannon in leftCannons)
            {
                ObjectPooler.GetInstance().SpawnFromPool(projectile,leftCannon);
            }
            leftCannonsCooldown = 0;
        }

        public void ShootRightCannons()
        {
            if (rightCannonsCooldown < cannonsCooldown)
                return;

            foreach (Transform rightCannon in rightCannons)
            {
                ObjectPooler.GetInstance().SpawnFromPool(projectile,rightCannon);
            }
            rightCannonsCooldown = 0;
        }
    }
}

