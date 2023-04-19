using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ObjectPooling
{
    public class GameObjectFactory<T> : IObjectFactory<T> where T : MonoBehaviour, IPoolable
    {
        private readonly T _prefab;

        public GameObjectFactory(T prefab)
        {
            _prefab = prefab;
        }

        public T Create()
        {
            return GameObject.Instantiate(_prefab);
        }
    }
}
