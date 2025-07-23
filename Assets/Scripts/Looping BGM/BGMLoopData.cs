using System;
using UnityEngine;

namespace LoopingBGM
{
    [Serializable]
    public class BGMLoopData
    {
        [SerializeField]
        private AudioClip bgm;

        [SerializeField]
        private float loopStart;

        [SerializeField]
        private float loopEnd;

        public AudioClip BGM       => bgm;
        public float     LoopStart => loopStart;
        public float     LoopEnd   => loopEnd;
    }
}