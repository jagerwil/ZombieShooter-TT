using Ducksten.Core;
using Ducksten.ZombieShooterTT.Events;
using UnityEngine;

namespace Ducksten.ZombieShooterTT.Characters {
    public class Player : Singleton<Player> {
        [SerializeField] private PlayerInputController _inputController;
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private CharacterHealth _health;
        [SerializeField] private PlayerWeaponHandler _weaponHandler;
        [SerializeField] private PlayerAnimationController _animationController;

        public PlayerInputController InputController => _inputController;
        public CharacterHealth HealthComponent => _health;
        public PlayerWeaponHandler WeaponHandler => _weaponHandler;

        private void Start() {
            Reset();
        }

        private void OnEnable() {
            GameEvents.onGameRestarted.Subscribe(Reset);
            _health.onDeath += OnDeath;
        }

        private void OnDisable() {
            GameEvents.onGameRestarted.Unsubscribe(Reset);
            _health.onDeath -= OnDeath;
        }

        private void Reset() {
            _movement.Reset();
            _health.Reset();
            _weaponHandler.Reset();
            _animationController.Reset();
        }

        private void OnDeath(CharacterHealth instigator) {
            GameEvents.onGameEnded.Invoke();
        }
    }
}
