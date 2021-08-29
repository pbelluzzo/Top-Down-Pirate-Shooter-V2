using System.Collections;
using UnityEngine;

namespace Gameplay.ObjectPooling
{
    public class PoolVfx : MonoBehaviour, IPoolObject
    {
        [SerializeField] float destroyTime;
        [SerializeField] PoolType label;
        public PoolType GetLabel() => label;
  
        public void OnEnable()
        {
            StartCoroutine(ReturnToQueue(destroyTime));
        }

        private IEnumerator ReturnToQueue(float time)
        {
            yield return new WaitForSeconds(time);
            ObjectPooler.GetInstance().EnqueueObject(label, this.gameObject);
            gameObject.SetActive(false);
        }

    }
}
