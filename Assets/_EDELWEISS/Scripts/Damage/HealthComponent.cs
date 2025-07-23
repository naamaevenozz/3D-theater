using System;
using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.Damage
{
    public class HealthComponent : EdelMono
    {
        [SerializeField]
        public float currentHealth;

        [SerializeField]
        public float maxHealth = 100f;

        public HitAccumulator           HitAccumulator       { get; private set; }
        public InvincibilityTimeHandler InvincibilityHandler { get; private set; }

        public event Action<HealthChangeContext> HealthChanged;

        private void Awake()
        {
            HealthChanged =  delegate { };
            HealthChanged += GameEvents.HealthChanged;

            HitAccumulator            =  new();
            HitAccumulator.HitApplied += OnHitApplied;
            InjectHitAccumulator();

            InvincibilityHandler = new(this);

            InvincibilityHandler.InvincibilityStarted += () => HitAccumulator.AcceptHits = false;
            InvincibilityHandler.InvincibilityStopped += () => HitAccumulator.AcceptHits = true;
        }

        private void InjectHitAccumulator()
        {
            var dependencies = GetComponentsInChildren<IHittable>(true);

            foreach (var dependency in dependencies) dependency.InjectHitAccumulator(HitAccumulator);
        }

        private void OnHitApplied(HitContext hit) => ApplyDamage(hit.Damage);

        public void ApplyDamage(float damage)
        {
            float previous = currentHealth;
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);

            HealthChangeContext context = new(this, previous, currentHealth);

            HealthChanged?.Invoke(context);

            InvincibilityHandler.DoInvincibility();
        }

        public void Reset()
        {
            currentHealth = maxHealth;
        }
    }
}