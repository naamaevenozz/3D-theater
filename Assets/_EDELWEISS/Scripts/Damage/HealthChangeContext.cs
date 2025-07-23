using System.Collections.Generic;
using Edelweiss.Utils;

namespace Edelweiss.Damage
{
    public class HealthChangeContext : EventContextBase<HealthComponent>
    {
        public readonly float Previous;
        public readonly float Current;
        public readonly float Delta;

        public HealthChangeContext(HealthComponent healthComponent, float previous, float current) :
            base(healthComponent)
        {
            Previous = previous;
            Current  = current;
            Delta    = current - previous;
        }

        protected override List<(string, object)> GetDebugStringFields()
        {
            return new()
                   {
                       ("Sender", Sender.gameObject.name),
                       ("From", Previous),
                       ("To", Current),
                       ("Delta", Delta)
                   };
        }
    }
}