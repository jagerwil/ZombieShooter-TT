using System;
using Ducksten.ZombieShooterTT.Enemies;
using Ducksten.ZombieShooterTT.Events;
using UnityEngine;

namespace Ducksten.ZombieShooterTT {
    public class KilledEnemiesCounter : MonoBehaviour {
        private int _killedEnemiesAmount;

        public int KilledEnemiesAmount => _killedEnemiesAmount;

        public event Action<int> onKilledEnemiesAmountChanged;

        private void OnEnable() {
            GameEvents.onEnemyDied.Subscribe(EnemyDied);
            GameEvents.onGameRestarted.Subscribe(Reset);
        }

        private void OnDisable() {
            GameEvents.onEnemyDied.Unsubscribe(EnemyDied);
            GameEvents.onGameRestarted.Unsubscribe(Reset);
        }

        private void Reset() {
            _killedEnemiesAmount = 0;
            onKilledEnemiesAmountChanged?.Invoke(_killedEnemiesAmount);
        }

        private void EnemyDied(Enemy enemy) {
            _killedEnemiesAmount += 1;
            onKilledEnemiesAmountChanged?.Invoke(_killedEnemiesAmount);
        }
    }
}
