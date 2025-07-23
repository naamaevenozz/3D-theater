using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Edelweiss.Core
{
    [CreateAssetMenu(fileName = "Configuration", menuName = "Edelweiss/Configuration")]
    public partial class EdelweissConfiguration : ScriptableObject
    {
        private static EdelweissConfiguration _instance = null;

        public static EdelweissConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    EdelweissConfiguration[] loaded = Resources.LoadAll<EdelweissConfiguration>("");

                    _instance = loaded[0];
                    _instance.OnValidate();
                }

                return _instance;
            }
        }

        [SerializeField]
        private float playersZ = 0;

        public float PlayersZ => playersZ;

        [SerializeField]
        [Range(0, 2f)]
        private float shockwaveRecoveryTime = .5f;

        public float ShockwaveRecoveryTime => shockwaveRecoveryTime;

        [SerializeField]
        private float impulseThreshold = 1f;

        public float ImpulseThreshold => impulseThreshold;

        [SerializeField]
        [Range(0f, 2.5f)]
        private float impulseScale = 1f;

        public float ImpulseScale => impulseScale;

        [SerializeField]
        private float damageTextLifetime = .5f;

        [SerializeField]
        private float damageTextSpeed = 3f;

        [SerializeField]
        private float invincibilityTime = .5f;

        public float DamageTextLifetime => damageTextLifetime;
        public float DamageTextSpeed    => damageTextSpeed;
        public float InvincibilityTime  => invincibilityTime;

        [SerializeField]
        [AssetsOnly]
        private List<GameObject> mainGamePrefabs = new List<GameObject>();

        public IReadOnlyList<GameObject> MainGamePrefabs => mainGamePrefabs;

        [SerializeField]
        private Scene player1WinScene;

        [SerializeField]
        private Scene player2WinScene;

        public Scene Player1WinScene => player1WinScene;
        public Scene Player2WinScene => player2WinScene;

        private void OnValidate()
        {
        }
    }
}