using System;
using System.Collections;
using Edelweiss.Core.Sound;
using Edelweiss.Core.UI;
using Edelweiss.Damage;
using Edelweiss.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Edelweiss.Core
{
    public class GameManager : EdelSingleton<GameManager>
    {
        public enum GameState
        {
            None,
            StartMatch,
            Fight,
            EndMatch
        }

        private const string WIN_SCENE_NAME      = "WinScreen";
        private const string GAMEPLAY_SCENE_NAME = "OldCharacterSelect";

        private HealthComponent player1Health;
        private HealthComponent player2Health;
        private HealthBarUI     player1HealthBarUI;
        private HealthBarUI     player2HealthBarUI;

        public GameState CurrentState { get; private set; } = GameState.None;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            GameEvents.FightStart += () => LogInfo("Game Started");
        }

        public void RegisterPlayers(HealthComponent p1, HealthComponent p2, HealthBarUI bar1, HealthBarUI bar2)
        {
            player1Health      = p1;
            player2Health      = p2;
            player1HealthBarUI = bar1;
            player2HealthBarUI = bar2;

            InitializePlayers();
            StartMatch();
        }

        private void InitializePlayers()
        {
            if (player1Health == null || player2Health == null)
            {
                Debug.LogError("Missing players in GameManager");
                return;
            }

            player1Health.HealthChanged -= OnHealthChanged;
            player2Health.HealthChanged -= OnHealthChanged;

            player1Health.HealthChanged += OnHealthChanged;
            player2Health.HealthChanged += OnHealthChanged;
        }

        private void OnHealthChanged(HealthChangeContext context)
        {
            if (context.Sender == player1Health)
                player1HealthBarUI.UpdateHealth(context.Current, context.Sender.maxHealth);
            else if (context.Sender == player2Health)
                player2HealthBarUI.UpdateHealth(context.Current, context.Sender.maxHealth);

            if (context.Current <= 0 && CurrentState == GameState.Fight)
                EndMatch(context.Sender);
        }

        public void StartMatch()
        {
            CurrentState = GameState.StartMatch;

            player1Health.Reset();
            player2Health.Reset();

            CurrentState = GameState.Fight;
            GameEvents.FightStart?.Invoke();
        }

        private void EndMatch(HealthComponent loser)
        {
            CurrentState = GameState.EndMatch;

            string winner   = loser == player1Health ? "Player 2" : "Player 1";
            Scene  winScene = loser == player1Health ? Config.Player2WinScene : Config.Player1WinScene;

            SceneManager.LoadScene(winScene.name);
        }

        public void GoToMainScene()
        {
            SceneManager.LoadScene(GAMEPLAY_SCENE_NAME);
        }
    }
}