using System;
using Edelweiss.Damage;
using Edelweiss.Utils.Pool;

namespace Edelweiss.Core.UI
{
    public class DamageUIHandler : EdelMono
    {
        private IPool<DamageUIText> pool;

        private void Awake()
        {
            pool = DamageUITextPool.Instance;
        }

        private void OnEnable()
        {
            GameEvents.HitApplied += OnHitApplied;
        }

        private void OnDisable()
        {
            GameEvents.HitApplied -= OnHitApplied;
        }

        private void OnHitApplied(HitContext hit)
        {
            DamageUIText damageText = pool.Get();
            
            damageText.Initialize(hit);
            damageText.OnLifetimeEnd += () => pool.Return(damageText);
        }
    }
}