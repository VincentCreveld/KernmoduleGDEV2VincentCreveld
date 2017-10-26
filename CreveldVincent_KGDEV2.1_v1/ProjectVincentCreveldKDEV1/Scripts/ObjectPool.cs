using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool <T> where T : Component,  IPoolable  {

    public Stack<T> objectPool;

	public ObjectPool(int amt, Transform parent, GameObject prefab) {
        objectPool = new Stack<T>();
        GameObject o;
        for(int i = 0; i < amt; i++) {
            o = GameObject.Instantiate(prefab);
            o.transform.parent = parent;
            AddObj(o.GetComponent<T>());
        }
    }

    public void AddObj(T obj) {
        //if(obj.GetComponent<GameObject>() != null)
        obj.gameObject.SetActive(false);
        objectPool.Push(obj);
    }

    public T GetObj() {
        T dummy = null;
        if(objectPool.Peek() != null) {
            T o = objectPool.Pop();
            o.gameObject.SetActive(true);
            o.poolEvent += OnEvent;
            return o;
        }
        else {
            return dummy;
        }
    }

    public void OnEvent(GameObject ip) {
        ip.GetComponent<T>().poolEvent -= OnEvent;
        AddObj(ip.GetComponent<T>());
    }
}
