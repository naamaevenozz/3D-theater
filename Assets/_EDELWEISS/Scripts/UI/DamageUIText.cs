using System;
using Edelweiss.Damage;
using Edelweiss.Utils;
using Edelweiss.Utils.Pool;
using ImprovedTimers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Edelweiss.Core.UI
{
    public class DamageUIText : EdelMono, IPoolable
    {
        [Required]
        [SerializeField]
        private TextMeshProUGUI _label;

        private Camera _camera;

        private CountdownTimer _lifetimeTimer;

        public event Action OnLifetimeStart;
        public event Action OnLifetimeEnd;


        private void Awake()
        {
            _lifetimeTimer = new CountdownTimer(Config.DamageTextLifetime);

            _lifetimeTimer.OnTimerStart += () => OnLifetimeStart?.Invoke();
            _lifetimeTimer.OnTimerStop  += () => OnLifetimeEnd?.Invoke();

            Reset();
        }

        public void Initialize(HitContext hit)
        {
            Vector3 worldPosition  = hit.Point;
            Vector3 screenPosition = _camera.WorldToScreenPoint(worldPosition);

            transform.position = screenPosition;

            int damage = (int)hit.Damage;

            _label.text = Format(new RomanNumeral(damage));

            _lifetimeTimer.Start();
        }

        private void Update()
        {
            if (!_lifetimeTimer.IsRunning) return;
            float newY = transform.position.y + (Config.DamageTextSpeed * Time.deltaTime);

            transform.position = transform.position.With(y: newY);
        }

        public void Reset()
        {
            _label.text = string.Empty;
            _camera     = Camera.main;

            _lifetimeTimer.Reset();

            OnLifetimeStart = delegate { };
            OnLifetimeEnd   = delegate { };
        }

        public string Format(RomanNumeral romanNumeral)
        {
            string result = string.Empty;

            float size  = 90f;
            float delta = 10f;

            foreach (string group in romanNumeral.Numerals)
            {
                string addition = $"<size={size}%>{group}</size>";
                result = addition + result;

                size += delta;
            }

            return result;
        }
    }
}