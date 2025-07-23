using System;
using System.Collections.Generic;
using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.Utils.Pool {
    public class EdelMultiPool<T> : EdelSingleton<EdelMultiPool<T>> where T : EdelMono, IPoolable
    {
        [SerializeField] private List<Entry> poolEntries;

        private Dictionary<string, BaseEdelweissPool<T>> _baseMonoPools;

        private void Awake()
        {
            _baseMonoPools = new Dictionary<string, BaseEdelweissPool<T>>();

            poolEntries.ForEach(RegisterPool);
        }

        private void RegisterPool(EdelMultiPool<T>.Entry entry)
        {
            BaseEdelweissPool<T> pool = new BaseEdelweissPool<T>(this, entry.Settings);
            _baseMonoPools.Add(entry.Name, pool);
        }

        public IPool<T> GetPool(string poolName) => _baseMonoPools[poolName];


        [System.Serializable]
        private class Entry
        {
            [SerializeField] private string name;
            public string Name => name;
            [SerializeField] private BaseEdelweissPool<T>.Settings settings;
            public BaseEdelweissPool<T>.Settings Settings => settings;
        }
    }
    }