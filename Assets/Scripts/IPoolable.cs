using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ObjectPooling
{
    public interface IPoolable<T> where T : MonoBehaviour
    {
        void OnGet();
        void OnReturn();

        GameObject GameObject { get; }
    }

}
