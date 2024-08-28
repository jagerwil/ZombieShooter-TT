using System.Collections.Generic;
using Ducksten.Core.Timer;
using Ducksten.ZombieShooterTT.Enemies;
using Ducksten.ZombieShooterTT.Events;
using Unity.Mathematics;
using UnityEngine;

namespace Ducksten.ZombieShooterTT {
    public class EnemiesController : MonoBehaviour {
        [SerializeField] private EnemySpawner _spawner;
        [SerializeField] private int _maxEnemiesAlive;
        [Space]
        [SerializeField] private float _initialIntervalBetweenSpawns = 1f;
        [SerializeField] private float _intervalMultiplierPerSecond = 0.99f;
        [SerializeField] private float _timeBetweenRecalculations = 10f;

        private List<Enemy> _enemies = new();
        private RepeatedTimer _recalculationTimer;

        private void Start() {
            _recalculationTimer = TimerManager.Instance.GetTimer();
            Reset();
        }

        private void OnEnable() {
            GameEvents.onEnemyDied.Subscribe(EnemyDied);
            GameEvents.onGameRestarted.Subscribe(Reset);
        }

        private void OnDisable() {
            GameEvents.onEnemyDied.Unsubscribe(EnemyDied);
            GameEvents.onGameRestarted.Unsubscribe(Reset);
        }

        private void Reset() {
            foreach (var enemy in _enemies) {
                enemy.Despawn();
            }
            _enemies.Clear();

            _recalculationTimer.Stop();
            _recalculationTimer.Init(_timeBetweenRecalculations, RecalculateTimeBetweenSpawns, false);
            _recalculationTimer.Start();
            
            _spawner.Init(_initialIntervalBetweenSpawns, EnemySpawned);
            RecalculateTimeBetweenSpawns();
            _spawner.StartSpawning();
        }

        private void RecalculateTimeBetweenSpawns() {
            var timePassed = GameTimeController.Instance.GetTimeSinceLevelStart().TotalSeconds;
            var timeBetweenSpawns = GetTimeBetweenSpawns(timePassed);
            _spawner.SetTimeBetweenSpawns(timeBetweenSpawns);
        }

        private float GetTimeBetweenSpawns(double timePassed) {
            return _initialIntervalBetweenSpawns * (float)math.pow(_intervalMultiplierPerSecond, timePassed);
        }

        private void EnemySpawned(Enemy enemy) {
            enemy.HealthComponent.onDeath += EnemyDeath;
            _enemies.Add(enemy);
            if (_enemies.Count >= _maxEnemiesAlive) {
                _spawner.StopSpawning();
            }

            void EnemyDeath(CharacterHealth instigator) {
                EnemyDied(enemy);
                enemy.HealthComponent.onDeath -= EnemyDeath;
            }
        }

        private void EnemyDied(Enemy enemy) {
            _enemies.Remove(enemy);
            if (_enemies.Count == _maxEnemiesAlive - 1) {
                _spawner.StartSpawning();
            }
        }
    }
}

