using System.Collections.Generic;
using UnityEngine;

namespace Ducksten.Core.Timer {
    public class TimerManager : Singleton<TimerManager> {
        private List<RepeatedTimer> _timers = new();

        private void Update() {
            var deltaTime = Time.deltaTime;
            foreach (var timer in _timers) {
                timer.Tick(deltaTime);
            }
        }

        public RepeatedTimer GetTimer() {
            var timer = new RepeatedTimer(TimerDespawned);
            _timers.Add(timer);
            return timer;
        }

        private void TimerDespawned(RepeatedTimer timer) {
            _timers.Remove(timer);
        }
    }
}
