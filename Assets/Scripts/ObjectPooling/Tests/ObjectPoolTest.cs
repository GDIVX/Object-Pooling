using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ObjectPooling;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectPoolTest
{
    ObjectPool<PoolObject> objectPool;

    [SetUp]
    public void Setup()
    {
        //create a prefab
        GameObject prefab = new GameObject();
        PoolObject poolObject = prefab.AddComponent<PoolObject>();

        //create a pool
        objectPool = new ObjectPool<PoolObject>(poolObject);
        objectPool.Populate(10);


    }

    [Test]
    public void PopulatePoolTestPasses()
    {
        Assert.IsTrue(objectPool.Count == 10);
    }

    [Test]
    public void PoolMemberGetAndReturnTest()
    {
        //Get
        PoolObject poolObject = objectPool.Get();
        Assert.IsNotNull(poolObject, "Object is Null");
        Assert.IsTrue(poolObject.isActiveAndEnabled, "Object is not enabled");


        //Return
        objectPool.Return(poolObject);
        Assert.IsFalse(poolObject.isActiveAndEnabled, "Object is not disabled after returning");
    }

    [Test]
    public void EdgeCase_TryingToGetFromPoolOverPopulation()
    {
        for (int i = 0; i < 200; i++)
        {
            //Get
            PoolObject poolObject = objectPool.Get();
            Assert.IsNotNull(poolObject, $"Object is Null. Failed after {i} attempts");
            Assert.IsTrue(poolObject.isActiveAndEnabled, $"Object is not enabled. Failed after {i} attempts");
        }

    }

    [Test]
    public void TestReturnAllObjects()
    {
        List<PoolObject> activeObjects = new List<PoolObject>();
        for (int i = 0; i < 5; i++)
        {
            activeObjects.Add(objectPool.Get());
        }

        objectPool.ReturnAll();

        foreach (PoolObject poolObject in activeObjects)
        {
            Assert.IsFalse(poolObject.isActiveAndEnabled, "Not all objects are inactive after ReturnAll");
        }
    }

    [Test]
    public void TestClearPool()
    {
        objectPool.Clear();
        Assert.AreEqual(0, objectPool.Count, "Pool count is not 0 after clearing the pool");
    }

    [Test]
    public void TestPoolGrowth()
    {
        for (int i = 0; i < 15; i++)
        {
            objectPool.Get();
        }

        Assert.AreEqual(15, objectPool.Count, "Pool did not grow as expected");
    }

    [Test]
    public void TestOnGetOnReturn()
    {

        PoolObject poolObject = objectPool.Get();
        Assert.IsTrue(poolObject.OnGetCalled, "OnGet method was not called");

        objectPool.Return(poolObject);
        Assert.IsTrue(poolObject.OnReturnCalled, "OnReturn method was not called");
    }

    [Test]
    public void TestReturningExternalObject()
    {
        GameObject externalObject = new GameObject();
        PoolObject externalPoolObject = externalObject.AddComponent<PoolObject>();

        LogAssert.Expect(LogType.Warning, "ObjectPool: Attempted to return a IPoolable that was not instantiated by this pool." +
            "\n Avoid doing this, as this can lead to contaminating the pool with incompatible game objects");

        objectPool.Return(externalPoolObject);
    }

    [Test]
    public void TestReturningObjectMultipleTimes()
    {
        PoolObject poolObject = objectPool.Get();
        objectPool.Return(poolObject);
        objectPool.Return(poolObject);

        // If necessary, add checks to validate the state of the pool after returning the object multiple times
    }

    [Test]
    public void TestGettingFromEmptyPool()
    {
        objectPool.Clear();
        PoolObject poolObject = objectPool.Get();
        Assert.IsNotNull(poolObject, "Object is null when getting from an empty pool");
        Assert.AreEqual(1, objectPool.Count, "Pool count is not 1 after getting an object from an empty pool");
    }

    [Test]
    public void TestClearPoolWithActiveObjects()
    {
        List<PoolObject> activeObjects = new List<PoolObject>();
        for (int i = 0; i < 5; i++)
        {
            activeObjects.Add(objectPool.Get());
        }

        objectPool.Clear();

        // If necessary, add checks to validate the state of the pool and the active objects after clearing the pool
    }

}
