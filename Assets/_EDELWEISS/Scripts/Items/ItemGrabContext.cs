using System.Collections.Generic;
using Edelweiss.Utils;
using UnityEngine;

namespace Edelweiss.Items
{
    public class ItemGrabContext : EventContextBase<HandCollector>
    {
        private readonly GrabbableItem item;

        public ItemGrabContext(HandCollector handCollector, GrabbableItem item) : base(handCollector)
        {
            this.item = item;
        }

        protected override List<(string, object)> GetDebugStringFields()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ItemReleaseContext : EventContextBase<HandCollector>
    {
        private readonly GrabbableItem item;

        public ItemReleaseContext(HandCollector sender, GrabbableItem item) : base(sender)
        {
            this.item = item;
        }

        protected override List<(string, object)> GetDebugStringFields()
        {
            throw new System.NotImplementedException(); 
        }
    }
}