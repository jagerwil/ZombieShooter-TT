using System;

namespace Ducksten.Core.Timer {
    public class RepeatedTimer {
        private float _interval;
        private int _repeatsAmount;
        private bool _isEndless;
        
        private Action<RepeatedTimer> _despawned;
        private Action _repeatedAction;
        private bool _skipFirstInterval;

        private int _repeatsLeft;
        private float _remainingTime;
        private bool _isActive;

        public bool IsActive => _isActive;

        public RepeatedTimer(Action<RepeatedTimer> despawnedCallback) {
            _despawned = despawnedCallback;
        }
        
        public void Init(float interval, Action repeatedAction, bool skipFirstInterval = true) {
            Init(interval, -1, repeatedAction, skipFirstInterval);
        }

        public void Init(float interval, int repeatsAmount, Action repeatedAction, bool skipFirstInterval = true) {
            _interval = interval;
            _repeatsAmount = repeatsAmount;
            _repeatedAction = repeatedAction;
            _skipFirstInterval = skipFirstInterval;
        }

        public void SetInterval(float newInterval) {
            _interval = newInterval;
        }

        public void Start() {
            if (_isActive) {
                return;
            }
            
            if (_repeatsAmount == 0) {
                Stop();
                return;
            }

            _remainingTime = _skipFirstInterval ? 0f : _interval;
            _repeatsLeft = _repeatsAmount;
            _isEndless = _repeatsAmount == -1;
            _isActive = true;
        }

        public void Stop() {
            _isActive = false;
        }

        public void Despawn() {
            _despawned?.Invoke(this);
        }

        public void Tick(float deltaTime) {
            if (!_isActive)
                return;

            _remainingTime -= deltaTime;
            if (_remainingTime > 0f) {
                return;
            }

            _repeatedAction?.Invoke();
            _remainingTime += _interval;

            if (_isEndless)
                return;

            _repeatsLeft -= 1;
            if (_repeatsLeft <= 0) {
                Stop();
            }
        }
    }
}
