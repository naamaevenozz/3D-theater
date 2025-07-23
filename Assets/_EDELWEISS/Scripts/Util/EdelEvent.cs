using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Edelweiss.Utils
{
    [Serializable]
    public class EdelEvent
    {
        [SerializeField]
        [HideLabel]
        private UnityEvent m_UnityEvent;

        private event Action m_ActionEvent;

        public EdelEvent()
        {
            m_ActionEvent = delegate { };
        }

        private void ValidateEvent()
        {
            if (m_ActionEvent != null) return;

            m_ActionEvent = delegate { };
        }

        public void Invoke()
        {
            m_ActionEvent?.Invoke();
            m_UnityEvent?.Invoke();
        }

        public static EdelEvent operator +(EdelEvent edelEvent, Action action)
        {
            edelEvent.ValidateEvent();
            edelEvent.m_ActionEvent += action;

            return edelEvent;
        }

        public static EdelEvent operator -(EdelEvent edelEvent, Action action)
        {
            edelEvent.ValidateEvent();
            edelEvent.m_ActionEvent -= action;

            return edelEvent;
        }
    }

    [System.Serializable]
    public class EdelEvent<T>
    {
        [SerializeField]
        [HideLabel]
        private UnityEvent<T> m_UnityEvent;

        private event Action<T> m_ActionEvent;

        public EdelEvent()
        {
            m_ActionEvent = delegate { };
        }

        private void ValidateEvent()
        {
            if (m_ActionEvent != null) return;

            m_ActionEvent = delegate { };
        }

        public void Invoke(T value)
        {
            m_ActionEvent?.Invoke(value);
            m_UnityEvent?.Invoke(value);
        }

        public static EdelEvent<T> operator +(EdelEvent<T> edelEvent, Action<T> action)
        {
            edelEvent.ValidateEvent();
            edelEvent.m_ActionEvent += action;

            return edelEvent;
        }

        public static EdelEvent<T> operator -(EdelEvent<T> edelEvent, Action<T> action)
        {
            edelEvent.ValidateEvent();
            edelEvent.m_ActionEvent -= action;

            return edelEvent;
        }
    }
}