using System.Collections.Generic;
using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.Utils.Pool
{
    class BaseEdelweissPool<T> : IPool<T> where T : EdelMono, IPoolable
    {
        private MonoBehaviour _monoBehaviour;

        private int totalCount;

        private          Stack<T> _available;
        private readonly Settings _poolSettings;

        public BaseEdelweissPool(MonoBehaviour mb, int initialSize, T prefab, Transform parent) : this(mb,
                 new Settings(initialSize, prefab, parent))
        {
        }

        public BaseEdelweissPool(MonoBehaviour mb, Settings settings)
        {
            _poolSettings       = settings;
            this._monoBehaviour = mb;

            _available = new Stack<T>();
            totalCount = 0;
            AddItemsToPool();
        }

        private void AddItemsToPool()
        {
            for (int i = 0; i < _poolSettings.InitialSize; i++)
            {
                T obj = Object.Instantiate(_poolSettings.Prefab, _poolSettings.Parent, true);

                obj.gameObject.SetActive(false);

                totalCount++;
                obj.gameObject.name = _poolSettings.Prefab.gameObject.name + "_" + totalCount;

                _available.Push(obj);
            }
        }

        public T Get()
        {
            return Get(Vector3.zero, Quaternion.identity);
        }

        public T Get(Vector3 position, Quaternion rotation)
        {
            if (_available.Count == 0) AddItemsToPool();

            T obj = _available.Pop();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);
            obj.Reset();

            return obj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_poolSettings.Parent);
            _available.Push(obj);
        }

        public void ReturnDelayed(T obj, float delay)
        {
            _monoBehaviour.StartCoroutine(ReturnDelayedCoroutine(obj, delay));
        }

        private System.Collections.IEnumerator ReturnDelayedCoroutine(T obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            Return(obj);
        }

        [System.Serializable]
        public class Settings
        {
            [SerializeField]
            private int _initialSize;

            [SerializeField]
            private T _prefab;

            [SerializeField]
            private Transform _parent;

            public int InitialSize
            {
                get => _initialSize;
                private set => _initialSize = value;
            }

            public T Prefab
            {
                get => _prefab;
                private set => _prefab = value;
            }

            public Transform Parent
            {
                get => _parent;
                private set => _parent = value;
            }

            public Settings(int initialSize, T prefab, Transform parent)
            {
                InitialSize = initialSize;
                Prefab      = prefab;
                Parent      = parent;
            }
        }
    }
}