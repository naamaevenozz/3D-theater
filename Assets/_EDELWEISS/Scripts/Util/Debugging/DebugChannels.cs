using System;
using System.Collections.Generic;

namespace Edelweiss.Utils.Debugging
{
    public static class DebugChannels
    {
        private static Dictionary<string, DebugChannel> s_Channels = new Dictionary<string, DebugChannel>();

        public static Action Subscribe(string channel, Action<object> action)
        {
            ValidateChannel(channel);

            return s_Channels[channel].Subscribe((obj) => action(obj));
        }

        private static void ValidateChannel(string channel)
        {
            if (!s_Channels.ContainsKey(channel))
            {
                s_Channels[channel] = new();
            }
        }

        public static void Broadcast(string channel, object message)
        {
            ValidateChannel(channel);

            s_Channels[channel].Broadcast(message?.ToString() ?? string.Empty);
        }
    }
}