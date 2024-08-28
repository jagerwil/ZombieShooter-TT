using System;

namespace Ducksten.ZombieShooterTT.Events {
    public class GameEvent {
        private event Action evt;

        public void Subscribe(Action action) {
            evt += action;
        }

        public void Unsubscribe(Action action) {
            evt -= action;
        }

        public void Invoke() {
            evt?.Invoke();
        }
    }
    
    public class GameEvent<T> {
        private event Action<T> evt;

        public void Subscribe(Action<T> action) {
            evt += action;
        }

        public void Unsubscribe(Action<T> action) {
            evt -= action;
        }

        public void Invoke(T param) {
            evt?.Invoke(param);
        }
    }
    
    public class GameEvent<T1, T2> {
        private event Action<T1, T2> evt;

        public void Subscribe(Action<T1, T2> action) {
            evt += action;
        }

        public void Unsubscribe(Action<T1, T2> action) {
            evt -= action;
        }

        public void Invoke(T1 param1, T2 param2) {
            evt?.Invoke(param1, param2);
        }
    }
}
