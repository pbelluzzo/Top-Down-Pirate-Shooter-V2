using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class ObjectPooler : MonoBehaviour
    {
        static ObjectPooler instance;
        public List<Pool> pools;
        
        Dictionary<PoolType, Queue<GameObject>> poolDictionary;
        Dictionary<PoolType, GameObject> poolPrefabs;

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
            poolPrefabs = new Dictionary<PoolType, GameObject>();

            foreach (Pool pool in pools)
            {
                poolPrefabs[pool.label] = pool.prefab;

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

        public GameObject SpawnFromPool(PoolType label, Transform spawnTransform, bool setParent = false)
        {
            GameObject objectToSpawn = GetObjectFromPool(label);


            objectToSpawn.transform.position = spawnTransform.position;
            objectToSpawn.transform.rotation = spawnTransform.rotation;

            if (setParent)
                objectToSpawn.transform.SetParent(spawnTransform);

            objectToSpawn.SetActive(true);

            return objectToSpawn;
        }

        GameObject GetObjectFromPool(PoolType label)
        {
            if (poolDictionary[label].Count <= 0)
            {
                GameObject toInstantiate = Instantiate(poolPrefabs[label]);
                EnqueueObject(label, toInstantiate);
            }

            GameObject objectToSpawn = poolDictionary[label].Dequeue();

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
