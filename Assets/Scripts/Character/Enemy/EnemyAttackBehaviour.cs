using System;
using Ducksten.Core.Timer;
using Ducksten.ZombieShooterTT.Characters;
using UnityEngine;

namespace Ducksten.ZombieShooterTT.Enemies {
    public class EnemyAttackBehaviour : MonoBehaviour {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private float _duration = 2f;
        [SerializeField] private int _damage = 10;
        [SerializeField] private float _damageDelay;
        [SerializeField] private float _delayBetweenAttacks = 0.2f;

        private RepeatedTimer _timer;
        private bool _attackStarted;
        private bool _canStartAttack = true;

        public event Action onAttackStarted;

        private void Start() {
            _timer = TimerManager.Instance.GetTimer();
        }

        public void Reset() {
            _timer?.Stop();
        }

        private void OnEnable() {
            _enemy.Pathfinding.onReachedPlayer += ReachedPlayer;
            _enemy.Pathfinding.onStartedMoving += StartedMoving;
        }

        private void OnDisable() {
            if (_enemy) {
                _enemy.Pathfinding.onReachedPlayer -= ReachedPlayer;
                _enemy.Pathfinding.onStartedMoving -= StartedMoving;
            }
        }

        private void ReachedPlayer() {
            if (_attackStarted) {
                return;
            }

            _attackStarted = true;
            StartAttack();
        }

        private void StartedMoving() {
            _attackStarted = false;
        }

        private void StartAttack() {
            if (!_canStartAttack) {
                return;
            }

            var playerHealth = Player.Instance.HealthComponent;
            if (!_attackStarted || playerHealth.IsDead) {
                _timer.Stop();
                return;
            }

            _canStartAttack = false;
            _enemy.Pathfinding.PausePathfinding();
            _timer.Init(_damageDelay, DealDamage, false);
            _timer.Start();
            onAttackStarted?.Invoke();
        }

        private void DealDamage() {
            var playerHealth = Player.Instance.HealthComponent;
            if (playerHealth.IsDead) {
                _timer.Stop();
                return;
            }

            playerHealth.TakeDamage(_enemy.HealthComponent, _damage);
            _timer.Init(_duration - _damageDelay, StartDelayBetweenAttacks, false);
        }

        private void StartDelayBetweenAttacks() {
            _timer.Init(_delayBetweenAttacks, DelayBetweenAttacksEnded, false);
            _enemy.Pathfinding.ResumePathfinding();
        }

        private void DelayBetweenAttacksEnded() {
            _canStartAttack = true;
            StartAttack();
        }
    }
}

