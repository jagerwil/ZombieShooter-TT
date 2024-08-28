using System;
using UnityEngine;

namespace Ducksten.ZombieShooterTT {
    public class CharacterHealth : MonoBehaviour {
        [SerializeField] private int _maxHealth;

        public delegate void HealthChangedDelegate(int oldHealth, int newHealth);

        private int _currentHealth;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;

        public bool IsDead => CurrentHealth <= 0;

        public event HealthChangedDelegate onHealthChanged;
        public event Action<CharacterHealth> onDeath;

        public void Reset() {
            SetHealth(_maxHealth);
        }

        public void TakeDamage(CharacterHealth instigator, int damage) {
            if (damage <= 0)
                return;

            SetHealth(_currentHealth - damage);
            if (_currentHealth == 0) {
                onDeath?.Invoke(instigator);
            }
        }

        private void SetHealth(int health) {
            var prevHealth = _currentHealth;
            _currentHealth = Mathf.Clamp(health, 0, _maxHealth);
            onHealthChanged?.Invoke(prevHealth, _currentHealth);
        }
    }
}
