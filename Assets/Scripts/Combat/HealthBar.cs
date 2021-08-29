using UnityEngine;
using UnityEngine.UI;
using Gameplay;

namespace Combat
{
    public class HealthBar : MonoBehaviour, IPoolObject
    {
        [SerializeField] float barOffset;
        [SerializeField] PoolType label;

        Transform shipTransform;

        Slider slider;

        public PoolType GetLabel() => label;
        public void SetMaxHealth(int maxHealth) => slider.maxValue = maxHealth;
        public void SetHealth(int health) => slider.value = health;
        public void SetShipTransform(Transform transform) => shipTransform = transform;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void LateUpdate()
        {
            UpdateBarPosition();
        }

        private void UpdateBarPosition()
        {
            Vector2 newPos = shipTransform.position;
            newPos.y += barOffset;
            transform.position = newPos;
        }
    }

}
