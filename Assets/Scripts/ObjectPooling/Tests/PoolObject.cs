using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour, IPoolable
{
    public GameObject GameObject => gameObject;

    public bool OnGetCalled { get; internal set; }
    public bool OnReturnCalled { get; internal set; }

    private void Awake()
    {
        OnGetCalled = false;
        OnReturnCalled = false;
    }

    public void OnGet()
    {
        OnGetCalled = true;
    }

    public void OnReturn()
    {
        OnReturnCalled = true;
    }
}
