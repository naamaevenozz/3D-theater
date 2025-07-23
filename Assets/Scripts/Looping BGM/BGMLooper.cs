using Sirenix.OdinInspector;
using UnityEngine;

namespace LoopingBGM
{
    public class BGMLooper : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private BGMSoundBank soundBank;

        private BGMLoopData currentLoopData;
        private AudioClip   currentBGM;
        private bool        isBGMInSoundBank;
        private float       loopLength;

        private void Start()
        {
            if (audioSource == null)
            {
                Debug.LogError("BGMLooper: AudioSource is not assigned.", this);
                enabled = false;
            }
        }

        void OnBGMChanged(AudioClip bgm)
        {
            if (bgm == null)
            {
                isBGMInSoundBank = false;
                currentBGM       = null;
                loopLength       = 0;
                return;
            }

            isBGMInSoundBank = soundBank.TryGetBGMData(bgm, out BGMLoopData loopData);
            if (!isBGMInSoundBank) return;

            currentLoopData = loopData;
            currentBGM      = bgm;
            loopLength      = loopData.LoopEnd - loopData.LoopStart;
        }

        private void Update()
        {
            if (currentBGM != audioSource.clip) OnBGMChanged(audioSource.clip);

            HandleLoop();
        }

        private void HandleLoop()
        {
            if (!isBGMInSoundBank) return;

            float bgmTime = audioSource.time;
            if (!(bgmTime >= currentLoopData.LoopEnd)) return;

            audioSource.time = bgmTime - loopLength;
        }
    }
}