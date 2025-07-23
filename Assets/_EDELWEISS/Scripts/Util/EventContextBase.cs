using System.Collections.Generic;
using System.Text;

namespace Edelweiss.Utils
{
    public abstract class EventContextBase<TSender>
    {
        private readonly StringBuilder debugStringBuilder = new StringBuilder();

        protected abstract List<(string, object)> GetDebugStringFields();

        public TSender Sender { get; }

        public EventContextBase(TSender sender)
        {
            Sender = sender;
        }

        public string DebugString(bool multiline = false)
        {
            string delimiter = multiline ? "\n" : ", ";

            var sb = debugStringBuilder;
            sb.Clear();

            if (!multiline) sb.Append(this.GetType().Name).Append(" { ");

            bool firstEntry = true;
            foreach ((string key, object value) p in GetDebugStringFields())
            {
                if (!firstEntry)
                {
                    sb.Append(delimiter);
                }

                sb.Append($"{p.key}:{p.value}");

                if (firstEntry) firstEntry = false;
            }

            if (!multiline) sb.Append(" }");

            return sb.ToString();
        }
    }
}