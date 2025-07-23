using System.Collections;
using Edelweiss.Utils.Pool;
using UnityEngine;

namespace Edelweiss.Core.Sound
{
    public class SoundManager : EdelMono
    {
        [Header("Audio Sources")] [SerializeField]
        public AudioSource musicSource;

        private static SoundManager _instance;
        public static SoundManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }


        //Play a one-shot SFX using the AudioSource pool
        public void PlaySound(AudioClip audioClip)
        {
            if (audioClip == null) return;

            // Get a pooled audio source
            var pooledAudioSource = AudioSourcePool.Instance.Get();
            pooledAudioSource.PlaySound(audioClip);
        }

        public void PlaySoundInDelay(AudioClip audioClip, float delay)
        {
            if (audioClip == null) return;
            StartCoroutine(PlaySoundAfterDelay(audioClip, delay));
        }

        private IEnumerator PlaySoundAfterDelay(AudioClip audioClip, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlaySound(audioClip);
        }

        //Play/Stop background music
        public void PlayMusic(AudioClip musicClip, bool loop = true, float volume = 1f)
        {
            if (musicClip == null) return;

            musicSource.Stop();
            musicSource.loop = loop;
            musicSource.volume = volume;
            musicSource.clip = musicClip;
            musicSource.Play();
        }

        //Stop current music 
        public void StopMusic()
        {
            musicSource.Stop();
        }
    }
}