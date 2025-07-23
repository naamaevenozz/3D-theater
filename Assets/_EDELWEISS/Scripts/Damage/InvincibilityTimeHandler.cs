using System;
using Edelweiss.Core;
using ImprovedTimers;

namespace Edelweiss.Damage
{
    public class InvincibilityTimeHandler
    {
        private readonly EdelweissConfiguration config;

        private readonly HealthComponent owner;
        private readonly HitAccumulator  hitAccumulator;

        private readonly CountdownTimer timer;

        public event Action InvincibilityStarted;
        public event Action InvincibilityStopped;

        public InvincibilityTimeHandler(HealthComponent owner)
        {
            config = EdelweissConfiguration.Instance;

            this.owner     = owner;
            hitAccumulator = owner.HitAccumulator;

            timer = new CountdownTimer(config.InvincibilityTime);

            timer.OnTimerStart += () => { InvincibilityStarted?.Invoke(); };
            timer.OnTimerStop  += () => { InvincibilityStopped?.Invoke(); };
        }

        public void DoInvincibility()
        {
            if (timer.IsRunning) return;

            timer.Reset(config.InvincibilityTime);
            timer.Start();
        }
    }
}