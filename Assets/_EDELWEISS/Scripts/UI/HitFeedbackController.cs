using Edelweiss.Core;
using Edelweiss.Damage;
using Edelweiss.Utils;
using UnityEngine;

namespace Edelweiss.UI
{
    public class HitFeedbackController : EdelMono
    {
        [SerializeField]
        private EdelEvent onHitAppliedEvent = new();

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
            Vector3 hitPosition = context.Point;
            onHitAppliedEvent.Invoke();
        }
    }
}