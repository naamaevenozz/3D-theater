using System;

namespace Edelweiss.Utils.Debugging
{
    internal class DebugChannel
    {
        private Action<string> m_OnMessageReceived;

        public string LastMessage { get; private set; }

        public DebugChannel()
        {
            m_OnMessageReceived = delegate { };
            LastMessage         = string.Empty;
        }

        public Action Subscribe(Action<string> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            m_OnMessageReceived += action;

            action(LastMessage);

            return () => m_OnMessageReceived -= action;
        }

        public void Broadcast(string message)
        {
            m_OnMessageReceived?.Invoke(message);
        }
    }
}