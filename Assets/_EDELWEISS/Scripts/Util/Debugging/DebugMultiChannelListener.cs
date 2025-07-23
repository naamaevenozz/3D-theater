using System;
using System.Collections.Generic;
using System.Text;
using Edelweiss.Core;
using Edelweiss.Utils;
using Edelweiss.Utils.Debugging;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Edelweiss.Utils.Debuggging
{
    public class DebugMultiChannelListener : EdelMono
    {
        [Serializable]
        public class Entry
        {
            [SerializeField]
            [DelayedProperty]
            [OnValueChanged("Subscribe")]
            private string m_Channel;

            [SerializeField]
            private string m_Prefix;

            private Action m_UnsubscribeAction;

            public string Output { get; private set; } = "";

            public void Subscribe()
            {
                m_UnsubscribeAction?.Invoke();
                m_UnsubscribeAction = DebugChannels.Subscribe(m_Channel, OnMessageReceived);
            }

            public void Unsubscribe()
            {
                m_UnsubscribeAction?.Invoke();
                m_UnsubscribeAction = null;
            }

            private void OnMessageReceived(object obj)
            {
                Output = string.IsNullOrEmpty(m_Prefix) ? obj.ToString() : $"{m_Prefix}: {obj}";
            }
        }

        [SerializeField]
        [TableList]
        private List<Entry> m_Channels = new();

        [Space(50)]
        [SerializeField]
        [InlineProperty]
        [HideLabel]
        private EdelEvent<string> OnUpdate = new();

        private StringBuilder _sb = new();

        private void OnEnable()
        {
            m_Channels.ForEach(entry => entry.Subscribe());
        }

        private void OnDisable()
        {
            m_Channels.ForEach(entry => entry.Unsubscribe());
        }

        private void Update()
        {
            _sb.Clear();

            m_Channels.ForEach(entry => _sb.AppendLine(entry.Output));

            OnUpdate?.Invoke(_sb.ToString());
        }
    }
}