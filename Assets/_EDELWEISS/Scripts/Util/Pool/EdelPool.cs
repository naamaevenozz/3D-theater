using System;
using System.Collections.Generic;
using Edelweiss.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Edelweiss.Utils.Pool
{
    [DefaultExecutionOrder(-100)]
    public class EdelPool<T> : EdelSingleton<EdelPool<T>>, IPool<T> where T : EdelMono, IPoolable
    {
        [SerializeField]
        private int initialSize;

        [SerializeField]
        private T prefab;

        [SerializeField]
        private Transform parent;

        private BaseEdelweissPool<T> _baseEdelweissPool;

        private void Awake()
        {
            _baseEdelweissPool = new BaseEdelweissPool<T>(this, initialSize, prefab, parent);
        }

        public T Get()
        {
            T obj = _baseEdelweissPool.Get();
            OnGet(obj);
            return obj;
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            T obj = _baseEdelweissPool.Get(position, rotation);
            OnGet(obj);
            return obj;
        }

        protected virtual void OnGet(T obj)
        {
        }

        public void Return(T obj) => _baseEdelweissPool.Return(obj);

        public void ReturnDelayed(T obj, float delay) => _baseEdelweissPool.ReturnDelayed(obj, delay);
    }
}