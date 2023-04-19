using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPoolDictionary<T> where T : MonoBehaviour , IPoolable
    {
        Dictionary<string, ObjectPool<T>> pools = new();

        
        public IPoolable Get(string key)
        {
            if (!pools.ContainsKey(key))
            {
                Debug.LogError("No pool with key " + key);
                return null;
            }
            return pools[key].Get();
        }

        public void Add(string key, T prefab)
        {
            if (pools.ContainsKey(key))
            {
                Debug.LogError("Pool with key " + key + " already exists");
                return;
            }
            pools.Add(key, new ObjectPool<T>(prefab));
        }

        public void Populate(string key, int count)
        {
            if (!pools.ContainsKey(key))
            {
                Debug.LogError("No pool with key " + key);
                return;
            }
            pools[key].Populate(count);
        }

        public void Return(string key, T poolable)
        {
            if (!pools.ContainsKey(key))
            {
                //Create a new pool
                Add(key, poolable);
                return;
            }
            pools[key].Return(poolable);
        }

        public void ReturnAll(string key)
        {
            if (!pools.ContainsKey(key))
            {
                Debug.LogError("No pool with key " + key);
                return;
            }
            pools[key].ReturnAll();
        }

        public void Clear(string key)
        {
            if (!pools.ContainsKey(key))
            {
                Debug.LogError("No pool with key " + key);
                return;
            }
            pools[key].Clear();
        }

        public void ClearAll()
        {
            foreach (ObjectPool<T> pool in pools.Values)
            {
                pool.Clear();
            }
            pools.Clear();
        }

        internal bool ContainsKey(string key)
        {
            return pools.ContainsKey(key);
        }
    }
}