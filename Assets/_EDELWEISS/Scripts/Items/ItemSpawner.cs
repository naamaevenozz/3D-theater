using System;
using System.Linq;
using Edelweiss.Core;
using UnityEngine;

namespace Edelweiss.Items
{
    [Serializable]
    public struct WeightedItem
    {
        [Tooltip("The item prefab to spawn")]
        public GameObject prefab;
        [Tooltip("Relative chance this item will be chosen")]
        public int       weight;
    }

    public class ItemSpawner : EdelMono
    {
        [Header("Spawned Items (weighted)")]
        [SerializeField]
        private WeightedItem[] items;

        [Header("Spawn Points")]
        [SerializeField]
        private Transform[]    spawnPoints;

        [Header("Time parameters")] [SerializeField]
        private float minCooldown = 10;
        private float maxCooldown = 30;

        private float spawnTimer;
        private float currentCooldown;
        private bool  isGameRunning;

        private void OnEnable()
        {
            GameEvents.FightStart += () => isGameRunning = true;
            GameEvents.FightEnd   += () => isGameRunning = false;
        }

        private void Start()
        {
            SetNewCooldown();
            GameEvents.FightStart?.Invoke();
        }

        private void Update()
        {
            if (!isGameRunning) return;

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= currentCooldown)
            {
                SpawnItem();
                spawnTimer = 0f;
                SetNewCooldown();
            }
        }

        private void SetNewCooldown()
        {
            currentCooldown = UnityEngine.Random.Range(minCooldown, maxCooldown);
        }

        private void SpawnItem()
        {
            var prefab = ChooseWeightedItem();
            var spawnPt = spawnPoints[ UnityEngine.Random.Range(0, spawnPoints.Length) ];
            Instantiate(prefab, spawnPt.position, spawnPt.rotation);
        }

        private GameObject ChooseWeightedItem()
        {
            int total = items.Sum(i => Math.Max(0, i.weight));
            if (total <= 0)
            {
                Debug.LogWarning("ItemSpawner: total weight is zero, falling back to first item.");
                return items.Length > 0 ? items[0].prefab : null;
            }
            
            int r = UnityEngine.Random.Range(1, total + 1);
            int accum = 0;
            
            foreach (var w in items)
            {
                accum += Math.Max(0, w.weight);
                if (r <= accum)
                    return w.prefab;
            }

            //fallback
            return items[^1].prefab;
        }
    }
}
