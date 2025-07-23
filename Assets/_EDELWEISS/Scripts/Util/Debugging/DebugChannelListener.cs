using System;
using Edelweiss.Core;
using Edelweiss.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Edelweiss.Utils.Debugging
{
    public class DebugChannelListener : EdelMono
    {
        [SerializeField]
        [DelayedProperty]
        [OnValueChanged("OnChannelChanged")]
        private string m_Channel;

        [SerializeField]
        private string m_Prefix;

        [SerializeField]
        [InlineProperty]
        [HideLabel]
        private EdelEvent<string> onBroadcast;

        private Action m_UnsubscribeAction;

        private void OnEnable()
        {
            m_UnsubscribeAction?.Invoke();
            m_UnsubscribeAction = DebugChannels.Subscribe(m_Channel, OnMessageReceived);
        }

        private void OnDisable()
        {
            m_UnsubscribeAction?.Invoke();
            m_UnsubscribeAction = null;
        }

        private void OnChannelChanged()
        {
            m_UnsubscribeAction?.Invoke();
            m_UnsubscribeAction = DebugChannels.Subscribe(m_Channel, OnMessageReceived);
        }

        private void OnMessageReceived(object obj)
        {
            string prefixedMessage = string.IsNullOrEmpty(m_Prefix) ? obj.ToString() : $"{m_Prefix}: {obj}";
            onBroadcast?.Invoke(prefixedMessage);
        }
    }
}