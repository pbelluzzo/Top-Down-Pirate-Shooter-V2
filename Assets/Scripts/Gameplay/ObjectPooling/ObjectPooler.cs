using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ObjectPooler : MonoBehaviour
    {
        static ObjectPooler instance;
        public List<Pool> pools;
        [SerializeField] Dictionary<PoolType, Queue<GameObject>> poolDictionary;

        public static ObjectPooler GetInstance() => instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;
        }

        void Start()
        {
            poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();

            foreach (Pool pool in pools)
            {
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                poolDictionary.Add(pool.label, objectPool);
            }
        }

        public GameObject SpawnFromPool(PoolType label, Vector3 spawnPos, Quaternion spawnRot)
        {
            if (poolDictionary[label].Count <= 0)
            {
                Debug.LogWarning("Queue with tag " + tag + " is empty");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[label].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = spawnPos;
            objectToSpawn.transform.rotation = spawnRot;

            return objectToSpawn;
        }
        public GameObject SpawnFromPool(PoolType label, Transform spawnTransform, bool setParent = false)
        {
            if (poolDictionary[label].Count <= 0)
            {
                Debug.LogWarning("Queue with tag " + tag + " is empty");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[label].Dequeue();

            objectToSpawn.transform.position = spawnTransform.position;
            objectToSpawn.transform.rotation = spawnTransform.rotation;

            if (setParent)
                objectToSpawn.transform.SetParent(spawnTransform);

            objectToSpawn.SetActive(true);

            return objectToSpawn;
        }


        public void EnqueueObject(PoolType label, GameObject objectToEnqueue)
        {
            if (!poolDictionary.ContainsKey(label))
            {
                Debug.LogWarning("Object pooler does not contain pool with desired label: " + label + 
                    ". Object \"" + objectToEnqueue.name + "\" not added to the pool");
                return;
            }

            poolDictionary[label].Enqueue(objectToEnqueue);
            objectToEnqueue.SetActive(false);
        }

    }

}
