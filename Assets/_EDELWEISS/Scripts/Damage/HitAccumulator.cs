using System;
using Edelweiss.Core;
using ImprovedTimers;
using UnityEngine;

namespace Edelweiss.Damage
{
    public class HitAccumulator
    {
        private const float DURATION_BUFFER = .08f;

        private HitContext     hit;
        private CountdownTimer bufferTimer;

        public bool AcceptHits     { get; set; }
        public bool IsBufferActive => bufferTimer is { IsRunning: true };

        public event Action BufferStart;
        public event Action BufferFinish;

        private readonly EDEEventManager events;

        public event Action<HitContext> HitApplied;

        public HitAccumulator()
        {
            events = EDEEventManager.Instance;

            AcceptHits = true;

            bufferTimer = new(DURATION_BUFFER);

            BufferStart  = delegate { };
            BufferFinish = delegate { };
            HitApplied   = delegate { };

            HitApplied += (hit)=>events.HitApplied.Invoke(hit);

            bufferTimer.OnTimerStart += () => BufferStart?.Invoke();
            bufferTimer.OnTimerStop  += () => BufferFinish?.Invoke();

            BufferFinish += OnBufferFinish;
        }

        private void OnBufferFinish()
        {
            HitApplied?.Invoke(hit);
    
            // events.HitApplied?.Invoke(hit);
            
            
            //HitApplied.Invoke(hit);
            hit = null;
        }

        public bool TryRegisterHit(DamageComponent damager, DamageablePart part, Collision collision)
        {
            if (!AcceptHits) return false;

            if (!bufferTimer.IsRunning) bufferTimer.Start();

            HitContext newHit = new HitContext(damager, part, collision);

            if (hit != null && hit.Damage >= newHit.Damage) return false;

            hit = newHit;
            events.HitRegistered?.Invoke(hit);
            return true;
        }
    }
}