using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ObjectPooling
{
    public class SpawnManager<T> where T : MonoBehaviour, IPoolable
    {
        private ObjectPool<T> pool;
        private IObjectFactory<T> factory;

        public SpawnManager(IObjectFactory<T> factory, int initialSize = 0)
        {
            this.factory = factory;
            T prefab = factory.Create();
            pool = new ObjectPool<T>(prefab);

            if (initialSize > 0)
            {
                pool.Populate(initialSize);
            }
        }

        public T Get()
        {
            T result = pool.Get();
            if (result == null)
            {
                result = factory.Create();
                pool.Return(result);
            }
            return result;
        }

        public void Return(T instance)
        {
            pool.Return(instance);
        }
    }

}
