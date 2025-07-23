using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.Utils.Pool
{
    public class PooledAudioSource : EdelMono, IPoolable
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();

            // Return to pool after the clip length
            Invoke(nameof(ReturnToPool), clip.length);
        }

        private void ReturnToPool()
        {
            AudioSourcePool.Instance.Return(this);
        }

        public void Reset()
        {
            // Reset audio properties for reuse
            _audioSource.Stop();
            _audioSource.clip = null;
        }
    }
}
