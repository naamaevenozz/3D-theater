using System;
using System.Collections.Generic;
using UnityEngine;
using static Edelweiss.Utils.ComponentExtensions;

namespace Edelweiss.Utils
{
    public class ComponentScan
    {
        private const bool DEFAULT_SELF     = true;
        private const bool DEFAULT_CHILDREN = false;
        private const bool DEFAULT_PARENTS  = false;

        private Transform m_Transform;
        private bool      m_IncludeSelf;
        private bool      m_IncludeChildren;
        private bool      m_IncludeParents;

        public event Action AfterScan = delegate { };

        internal ComponentScan()
        {
            Reset();
        }

        public void Reset()
        {
            m_Transform = null;

            m_IncludeSelf     = DEFAULT_SELF;
            m_IncludeChildren = DEFAULT_CHILDREN;
            m_IncludeParents  = DEFAULT_PARENTS;
        }

        public ComponentScan SetTransform(Transform transform)
        {
            m_Transform = transform;
            return this;
        }

        public ComponentScan ExcludeSelf()
        {
            m_IncludeSelf = false;
            return this;
        }

        public ComponentScan IncludeChildren()
        {
            m_IncludeChildren = true;
            return this;
        }

        public ComponentScan IncludeParents()
        {
            m_IncludeParents = true;
            return this;
        }

        public bool GetSingle<T>(out T outComponent)
        {
            bool result = ScanForComponent(m_Transform,
                                           out outComponent,
                                           m_IncludeSelf,
                                           m_IncludeParents,
                                           m_IncludeChildren);

            AfterScan?.Invoke();

            return result;
        }

        public bool SetSingle<T>(ref T component)
        {
            bool result = ScanForComponent(m_Transform,
                                           out T outComponent,
                                           m_IncludeSelf,
                                           m_IncludeParents,
                                           m_IncludeChildren);

            if (result)
            {
                component = outComponent;
            }

            AfterScan?.Invoke();

            return result;
        }

        public void DoSingle<T>(Action<T> action)
        {
            if (!GetSingle(out T component)) return;
            if (component == null) return;

            action?.Invoke(component);
        }

        public bool GetAll<T>(out T[] outComponents)
        {
            bool result = ScanForComponents(m_Transform,
                                            out outComponents,
                                            m_IncludeSelf,
                                            m_IncludeParents,
                                            m_IncludeChildren);
            AfterScan?.Invoke();

            return result;
        }

        public void DoAll<T>(Action<T[]> action)
        {
            if (!GetAll(out T[] components)) return;

            action?.Invoke(components);
        }

        public void DoAll<T>(Action<T> action)
        {
            if (!GetAll(out T[] components)) return;

            foreach (T component in components)
            {
                action?.Invoke(component);
            }
        }
    }

    public class ComponentScannerPool
    {
        private const int INITIAL_COUNT = 20;
        private const int RESIZE_COUNT  = 10;

        private static ComponentScannerPool instance;

        public static ComponentScannerPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComponentScannerPool();
                }

                return instance;
            }
        }

        private Queue<ComponentScan> m_Pool;

        public ComponentScannerPool()
        {
            m_Pool = new();
            Fill(INITIAL_COUNT);
        }

        private void Fill(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var scan = new ComponentScan();

                scan.AfterScan += () => Return(scan);

                m_Pool.Enqueue(scan);
            }
        }

        public ComponentScan Get()
        {
            if (m_Pool.Count == 0)
            {
                Fill(RESIZE_COUNT);
            }

            ComponentScan scanner = m_Pool.Dequeue();
            scanner.Reset();
            return scanner;
        }

        public void Return(ComponentScan scanner)
        {
            if (scanner == null) return;

            m_Pool.Enqueue(scanner);
        }
    }
}