using Ducksten.ZombieShooterTT.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ducksten.ZombieShooterTT.Ui {
    public class PlayerHealthView : MonoBehaviour {
        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_Text _text;

        private void OnEnable() {
            Player.Instance.HealthComponent.onHealthChanged += HealthChanged;
        }

        private void OnDisable() {
            var player = Player.Instance;
            if (player)
                player.HealthComponent.onHealthChanged -= HealthChanged;
        }

        private void Start() {
            var healthComponent = Player.Instance.HealthComponent;
            var health = healthComponent.CurrentHealth;
            var maxHealth = healthComponent.MaxHealth;
            UpdateHealthSlider(health, maxHealth);
        }

        private void HealthChanged(int oldHealth, int newHealth) {
            var maxHealth = Player.Instance.HealthComponent.MaxHealth;
            UpdateHealthSlider(newHealth, maxHealth);
        }

        private void UpdateHealthSlider(int health, int maxHealth) {
            _slider.value = (float)health / maxHealth;
            _text.text = $"{health}/{maxHealth}";
        }
    }
}
