using System.Collections;
using Ducksten.Core.ObjectPooling;
using Ducksten.ZombieShooterTT.Events;
using UnityEngine;

namespace Ducksten.ZombieShooterTT.Enemies {
    public class Enemy : PoolObject {
        [SerializeField] private CharacterHealth _health;
        [SerializeField] private EnemyPathfinding _pathfinding;
        [SerializeField] private EnemyAttackBehaviour _attackBehaviour;
        [SerializeField] private EnemyAnimationController _animationController;
        [Space]
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _delayBeforeDespawning = 5f;

        public CharacterHealth HealthComponent => _health;
        public EnemyPathfinding Pathfinding => _pathfinding;
        public EnemyAttackBehaviour AttackBehaviour => _attackBehaviour;
        public EnemyAnimationController AnimationController => _animationController;

        private void OnEnable() {
            _health.onDeath += OnDeath;
        }

        private void OnDisable() {
            _health.onDeath -= OnDeath;
        }

        public void Reset() {
            _collider.enabled = true;
            _rigidbody.isKinematic = false;
            _pathfinding.enabled = true;

            _health.Reset();
            _pathfinding.Reset();
            _attackBehaviour.Reset();
            _animationController.Reset();
        }

        private void OnDeath(CharacterHealth instigator) {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;
            _pathfinding.enabled = false;
            _attackBehaviour.Reset();
            _pathfinding.PausePathfinding();

            StartCoroutine(DespawnAfterDelay());
            GameEvents.onEnemyDied.Invoke(this);
        }

        private IEnumerator DespawnAfterDelay() {
            yield return new WaitForSeconds(_delayBeforeDespawning);
            Despawn();
        }
    }
}