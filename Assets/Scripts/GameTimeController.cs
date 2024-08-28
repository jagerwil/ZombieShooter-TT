using System;
using Ducksten.Core;
using Ducksten.ZombieShooterTT.Events;

namespace Ducksten.ZombieShooterTT {
    public class GameTimeController : Singleton<GameTimeController> {
        private DateTime _startingTime;

        private DateTime CurrentTime => DateTime.UtcNow;

        public TimeSpan GetTimeSinceLevelStart() {
            return CurrentTime - _startingTime;
        }

        private void Start() {
            Reset();
        }

        private void OnEnable() {
            GameEvents.onGameRestarted.Subscribe(Reset);
        }

        private void OnDisable() {
            GameEvents.onGameRestarted.Subscribe(Reset);
        }

        private void Reset() {
            _startingTime = CurrentTime;
        }
    }
}
