using System;
using Edelweiss.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Edelweiss.PhysicsSystem
{
    public class ShockwaveTester : EdelMono
    {
        [SerializeField]
        private Shockwave shockwave;

        [Button] public void TriggerShockwave() => shockwave.Trigger(transform.position);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shockwave.Radius);
        }
    }
}