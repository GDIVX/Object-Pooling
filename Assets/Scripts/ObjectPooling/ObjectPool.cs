using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{

    /// <summary>
    /// A class for managing the instantiation and recycling of GameObjects in Unity.
    /// </summary>
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private T prefab;
        private List<T> pool = new();

        public int Count => pool.Count;

        /// <summary>
        /// Initializes a new instance of the ObjectPool class.
        /// </summary>
        public ObjectPool(T prefab)
        {
            this.prefab = prefab;
        }

        /// <summary>
        /// Retrieves an inactive IPoolable from the pool or instantiates a new one if no inactive objects are available.
        /// </summary>
        /// <returns>An active IPoolable from the pool.</returns>
        public T Get()
        {
            T result = null;
            foreach (T poolable in pool)
            {
                if (!poolable.GameObject.activeSelf)
                {
                    result = poolable;
                    break;
                }
            }

            //If no inactive objects were found, instantiate a new one
            if (result == null)
            {
                GameObject gameObject = GameObject.Instantiate(prefab.GameObject);
                result = gameObject.AddComponent<T>();
                pool.Add(result);
            }
            
            result.GameObject.SetActive(true);
            result.OnGet();
            return result;
        }

        /// <summary>
        /// Sets a IPoolable as inactive and returns it to the pool.
        /// </summary>
        /// <param name="poolable">The IPoolable to be returned to the pool.</param>
        public void Return(T poolable)
        {
            if (pool.Contains(poolable))
            {
                poolable.OnReturn();
                poolable.GameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning("ObjectPool: Attempted to return a IPoolable that was not instantiated by this pool." +
                    "\n Avoid doing this, as this can lead to contaminating the pool with incompatible game objects");
            }
        }

        /// <summary>
        /// Sets all GameObjects in the pool as inactive.
        /// </summary>
        public void ReturnAll()
        {
            foreach (T poolable in pool)
            {
                Return(poolable);
            }
        }

        /// <summary>
        /// Destroys all GameObjects in the pool and clears the list.
        /// </summary>
        public void Clear()
        {
            foreach (IPoolable poolable in pool)
            {
                GameObject.Destroy(poolable.GameObject);
            }
            pool.Clear();
        }

        /// <summary>
        /// Pre-instantiates a specified number of GameObjects and adds them to the pool as inactive objects.
        /// </summary>
        /// <param name="count">The number of GameObjects to pre-instantiate.</param>
        public void Populate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject poolable = GameObject.Instantiate(prefab.GameObject);

                //Extract T from the instantiated GameObject
                T poolableComponent = poolable.GetComponent<T>();

                //If the instantiated GameObject does not have a T component, add one
                if (poolableComponent == null)
                {
                    poolableComponent = poolable.AddComponent<T>();
                }

                poolable.SetActive(false);
                pool.Add(poolableComponent);
            }
        }
    }
}