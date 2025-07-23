using Edelweiss.Core;
using Edelweiss.Damage;
using UnityEngine;

namespace Edelweiss.Utils.Debugging
{
    public class HitDebugger : EdelMono
    {
        private void OnEnable()
        {
            GameEvents.HitApplied += OnHitApplied;
        }

        private void OnDisable()
        {
            GameEvents.HitApplied -= OnHitApplied;
        }

        private void OnHitApplied(HitContext context)
        {
            transform.position = context.Point;
            Vector3 hitPos     = context.Point;
            Vector3 damagerPos = context.Damager.transform.position;
            Vector3 targetPos  = context.TargetPart.transform.position;

            LogInfo($"Hit applied at position: {hitPos}, Damage: {context.Damage}, Target: {context.TargetPart.name}, Damager: {context.Damager.name}");

            Debug.DrawLine(damagerPos, context.Damager.GetComponent<Rigidbody>().linearVelocity * Config.ImpulseScale,
                           Color.yellow);

            Debug.DrawLine(damagerPos, targetPos, Color.blue);

            Debug.DrawLine(damagerPos, damagerPos + context.RelativeVelocity * Config.ImpulseScale, Color.green);

            Debug.DrawLine(hitPos, context.Impulse + context.Point, Color.magenta);

            Debug.Break();
        }
    }
}