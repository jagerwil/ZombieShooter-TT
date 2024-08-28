using TMPro;
using UnityEngine;

namespace Ducksten.ZombieShooterTT.Ui {
    public class KilledEnemiesAmountView : MonoBehaviour {
        [SerializeField] private KilledEnemiesCounter _counter;
        [SerializeField] private TMP_Text _text;

        private void OnEnable() {
            _counter.onKilledEnemiesAmountChanged += KilledEnemiesAmountChanged;
        }

        private void OnDisable() {
            _counter.onKilledEnemiesAmountChanged -= KilledEnemiesAmountChanged;
        }

        private void Start() {
            KilledEnemiesAmountChanged(_counter.KilledEnemiesAmount);
        }

        private void KilledEnemiesAmountChanged(int amount) {
            _text.text = amount.ToString();
        }
    }
}
