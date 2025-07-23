using Edelweiss.Damage;
using UnityEngine;

namespace Edelweiss.Core.UI
{
    public class HealthUILoader : EdelMono
    {
        private void Start()
        {
            var player1 = GameObject.FindWithTag("Player1")?.GetComponent<HealthComponent>();
            var player2 = GameObject.FindWithTag("Player2")?.GetComponent<HealthComponent>();
            var bar1    = GameObject.Find("Player1HealthBar")?.GetComponent<HealthBarUI>();
            var bar2    = GameObject.Find("Player2HealthBar")?.GetComponent<HealthBarUI>();

            if (player1 == null || player2 == null || bar1 == null || bar2 == null)
            {
                LogError("Initialization failed: Missing components!");
            }

            GameManager.Instance.RegisterPlayers(player1, player2, bar1, bar2);
        }
    }
}