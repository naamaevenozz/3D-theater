using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LoopingBGM
{
    [CreateAssetMenu(fileName = "BGMSoundBank", menuName = "ScriptableObjects/Looping BGM/BGMSoundBank", order = 1)]
    public class BGMSoundBank : ScriptableObject
    {
        [SerializeField]
        private BGMLoopData[] bgmLoopData;

        private Dictionary<AudioClip, BGMLoopData> _bgmLoopDataDict;

        public bool TryGetBGMData(AudioClip bgm, out BGMLoopData data)
        {
            if (_bgmLoopDataDict == null) CreateDict();

            return _bgmLoopDataDict.TryGetValue(bgm, out data);
        }

        void CreateDict()
        {
            _bgmLoopDataDict = new();

            foreach (var data in bgmLoopData)
            {
                if (_bgmLoopDataDict.ContainsKey(data.BGM)) continue;

                _bgmLoopDataDict.Add(data.BGM, data);
            }
        }

        private void OnValidate() => CreateDict();
        private void OnEnable()   => CreateDict();
    }
}