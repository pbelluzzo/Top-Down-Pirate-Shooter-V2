using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class PoolVfx : MonoBehaviour, IPoolObject
    {
        [Min(0)]
        [SerializeField] float destroyTime;
        [SerializeField] PoolType label;
        public PoolType GetLabel() => label;
  
        public void OnEnable()
        {
            if (destroyTime > 0)
                StartCoroutine(ReturnToQueue(destroyTime));
        }

        private IEnumerator ReturnToQueue(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.transform.SetParent(null);
            ObjectPooler.GetInstance().EnqueueObject(label, this.gameObject);
        }

        public void ReturnToQueueImmediately()
        {
            gameObject.transform.SetParent(null);
            ObjectPooler.GetInstance().EnqueueObject(label, gameObject);
        }

    }
}
