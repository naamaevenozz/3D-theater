using System;
using ImprovedTimers;
using UnityEngine;

namespace Edelweiss.Core.FX
{
    public class KissCollisionFXManager : EdelMono
    {
        [Tooltip("Clip to play on kiss collision")]
        public AudioClip kissSound;

        private AudioSource audioSource;

        [SerializeField]
        private float cooldownTime = 1.5f;

        [SerializeField]
        private GameObject kissCollisionFXPrefab;

        private CountdownTimer _cooldownTimer;

        private void Awake()
        {
            audioSource    = GetComponent<AudioSource>();
            _cooldownTimer = new CountdownTimer(cooldownTime);
        }

        private void OnEnable()
        {
            EDEEventManager.Instance.OnKissCollision += OnKissCollision;
        }

        private void OnDisable()
        {
            EDEEventManager.Instance.OnKissCollision -= OnKissCollision;
        }

        private void OnKissCollision(Collider col)
        {
            if (_cooldownTimer is { IsRunning: true }) return;
            if (kissCollisionFXPrefab == null)
            {
                Debug.LogWarning("Kiss Collision FX Prefab is not assigned.");
                return;
            }

            _cooldownTimer.Reset(cooldownTime);
            _cooldownTimer.Start();

            var fx = Instantiate(kissCollisionFXPrefab);
            fx.transform.position = col.transform.position;

            var destroyTimer = new CountdownTimer(1);

            destroyTimer.OnTimerStop += () => Destroy(fx);
            destroyTimer.Start();

            if (kissSound != null)
                audioSource.PlayOneShot(kissSound);
            else
                Debug.LogWarning("No kiss sound assigned to KissCollisionSound.");
        }
    }
}