using Edelweiss.Core.Player;
using Edelweiss.Core;
using Edelweiss.Damage;
using Edelweiss.Utils;
using UnityEngine;

namespace Edelweiss.PhysicsSystem
{
    public class ShockwaveManager : EdelMono
    {
        [SerializeField]
        private float damageToRadiusScale = 1;

        [SerializeField]
        [Range(1, 20)]
        private float damageToRadiusRoot = 1;

        [SerializeField]
        private float damageToForceScale = 1;

        [SerializeField]
        [Range(1, 20)]
        private float damageToForceRoot = 1;

        [Range(0, 5f)]
        public float DecayRate = 0f;

        [SerializeField]
        private LayerMask affectedLayers;

        private void OnEnable()
        {
            GameEvents.HitApplied += OnHitApplied;
        }

        private void OnDisable()
        {
            GameEvents.HitApplied -= OnHitApplied;
        }

        void OnHitApplied(HitContext context)
        {
            Vector3 hitPosition     = context.Point;
            Vector3 damagerPosition = context.Damager.transform.position;
            Vector3 targetPosition  = context.TargetPart.transform.position;

            // context.Damager.ComponentScan()
            //        .IncludeParents()
            //        .DoSingle<RootController>((rc) => damagerPosition = rc.transform.position);
            // context.TargetPart.ComponentScan()
            //        .IncludeParents()
            //        .DoSingle<RootController>((rc) => damagerPosition = rc.transform.position);

            // Calculate the midpoint between the damager root,target root and hit positions

            Vector3 midpoint = (damagerPosition + targetPosition) / 2f;

            float radius = Mathf.Pow(context.Damage, 1f / damageToRadiusRoot) * damageToRadiusScale;
            float force  = Mathf.Pow(context.Damage, 1f / damageToForceRoot)  * damageToForceScale;

            Shockwave shockwave = new Shockwave(radius, force, affectedLayers);
            shockwave.DecayRate = DecayRate;

            shockwave.Trigger(midpoint);
        }
    }
}