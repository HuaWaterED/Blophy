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
    protected int sortSeed;
    protected ObjectPoolBase(T @object, int poolLength, int sortSeed, Transform parent = null)
    {
        this.parent = parent;
        this.sortSeed = sortSeed;
        PoolObject = @object;
    }
    protected T CreateObject()
    {
        T obj = Object.Instantiate(PoolObject, Vector3.zero, Quaternion.identity, parent == null ? PoolObject.transform : parent);
        NoteController note = obj.GetComponent<NoteController>();
        for (int i = 0; i < note.spriteRenderers.Count; i++)
        {
            foreach (var item in note.spriteRenderers[i].spriteRenderers)
            {
                item.sortingOrder = sortSeed + 1 + i;
            }
        }
        obj.gameObject.SetActive(false);
        return obj;
    }

    protected virtual T GetObject() => null;

    public virtual void ReturnObject(T obj)
    {
    }
}
