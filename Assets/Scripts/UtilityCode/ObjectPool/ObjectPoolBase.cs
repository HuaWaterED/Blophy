using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public abstract class ObjectPoolBase<T> where T : MonoBehaviour
{
    protected T PoolObject;
    protected Transform parent;
    protected Quaternion rotation;
    protected ObjectPoolBase(T @object, int poolLength, Quaternion quaternion, Transform parent = null)
    {
        this.parent = parent;
        PoolObject = @object;
    }
    protected T CreateObject()
    {
        T obj = Object.Instantiate(PoolObject, Vector3.zero, rotation, parent == null ? PoolObject.transform : parent);
        obj.gameObject.SetActive(false);
        return obj;
    }

    protected virtual T GetObject() => null;

    public virtual void ReturnObject(T obj)
    {
    }
}
