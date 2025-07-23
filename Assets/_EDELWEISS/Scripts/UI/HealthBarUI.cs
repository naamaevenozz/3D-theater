using System;
using Edelweiss.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Edelweiss.Core.UI
{
    public class HealthBarUI : EdelMono
    {
        private static readonly int IDFill     = Shader.PropertyToID("_Fill");
        private static readonly int IDBackFill = Shader.PropertyToID("_BackFill");

        [SerializeField]
        [Required]
        private Material _material;

        private float lastUpdateTime = -1f;

        [SerializeField]
        private float fillSpeed = 3f;

        [SerializeField]
        private float backFillDelay = 1f;

        [SerializeField]
        private float backFillSpeed = 0.5f;

        [ShowInInspector, ReadOnly]
        private float targetFill = 1f;

        [ShowInInspector, ReadOnly]
        private float targetBackFill = 1f;

        [ShowInInspector, ReadOnly]
        private float Fill
        {
            get => _material ? _material.GetFloat(IDFill) : 1f;
            set => _material?.SetFloat(IDFill, value);
        }

        [ShowInInspector, ReadOnly]
        private float BackFill
        {
            get => _material ? _material.GetFloat(IDBackFill) : 1f;
            set => _material?.SetFloat(IDBackFill, value);
        }

        public EdelEvent OnDamaged = new();

        public void UpdateHealth(float current, float max)
        {
            targetFill = Mathf.Clamp01(current / max);
            if (targetFill < Fill)
            {
                OnDamaged.Invoke();
            }

            lastUpdateTime = Time.time;
        }

        private void Update()
        {
            HandleFill();
            HandleBackFill();
        }

        private void HandleFill()
        {
            if (Mathf.Approximately(Fill, targetFill)) return;

            Fill           = Mathf.MoveTowards(Fill, targetFill, fillSpeed * Time.deltaTime);
            targetBackFill = Fill;

            lastUpdateTime = Time.time;
        }

        private void HandleBackFill()
        {
            if (Fill > BackFill) BackFill = Fill;

            if (Time.time < lastUpdateTime + backFillDelay) return;

            if (Mathf.Approximately(BackFill, targetBackFill)) return;

            BackFill = Mathf.MoveTowards(BackFill, targetBackFill, backFillSpeed * Time.deltaTime);
        }
    }
}