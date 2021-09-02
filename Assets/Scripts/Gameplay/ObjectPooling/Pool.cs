using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [System.Serializable]
    public class Pool
    {
        public PoolType label;
        public int size;
        public GameObject prefab;

        public Pool(PoolType poolLabel, int poolSize, GameObject poolPrefab)
        {
            label = poolLabel;
            size = poolSize;
            prefab = poolPrefab;
        }
    }
}