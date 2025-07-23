using System;
using System.Collections;
using Edelweiss.Core.UI;
using Edelweiss.Damage;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Edelweiss.Core
{
    public class GameLoader : EdelMono
    {
        [SerializeField] private string gameSceneName;
        [SerializeField] private string characterSelectionSceneName;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("Game Manager is null!");
                return;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(characterSelectionSceneName);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == gameSceneName)
                StartCoroutine(InitializeGame());
        }

        private IEnumerator InitializeGame()
        {
            yield return null;

            LoadMainGamePrefabs();

            var player1 = GameObject.FindWithTag("Player1")?.GetComponent<HealthComponent>();
            var player2 = GameObject.FindWithTag("Player2")?.GetComponent<HealthComponent>();
            var bar1    = GameObject.Find("Player1HealthBar")?.GetComponent<HealthBarUI>();
            var bar2    = GameObject.Find("Player2HealthBar")?.GetComponent<HealthBarUI>();

            if (player1 == null || player2 == null || bar1 == null || bar2 == null)
            {
                LogError("Initialization failed: Missing components!");
                yield break;
            }

            GameManager.Instance.RegisterPlayers(player1, player2, bar1, bar2);
        }

        private void LoadMainGamePrefabs()
        {
            foreach (GameObject prefab in Config.MainGamePrefabs)
            {
                if (GameObject.Find(prefab.name) != null)
                {
                    LogWarning($"Prefab {prefab.name} already exists in the scene. Skipping instantiation.");
                    continue;
                }

                GameObject instance = Instantiate(prefab);
            }
        }
    }
}