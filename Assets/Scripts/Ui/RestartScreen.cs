using Ducksten.ZombieShooterTT.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Ducksten.ZombieShooterTT.Ui {
    public class RestartScreen : MonoBehaviour {
        [SerializeField] private GameObject _root;
        [SerializeField] private Button _restartButton;

        private void Awake() {
            _restartButton.onClick.AddListener(RestartGame);
        }

        private void OnEnable() {
            GameEvents.onGameEnded.Subscribe(Show);
        }

        private void OnDisable() {
            GameEvents.onGameEnded.Unsubscribe(Show);
        }

        private void Show() {
            _root.SetActive(true);
        }

        private void Hide() {
            _root.SetActive(false);
        }

        private void RestartGame() {
            GameEvents.onGameRestarted?.Invoke();
            Hide();
        }
    }
}

