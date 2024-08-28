using UnityEngine;

namespace Ducksten.Core {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
        private static T _instance;
        public static T Instance {
            get {
                if (_instance)
                    return _instance;

                _instance = FindObjectOfType<T>();
                return _instance;
            }
        }

        protected virtual bool Awake() {
            if (_instance == this)
                return true;
            
            if (_instance) {
                Destroy(this);
                return false;
            }

            _instance = (T)this;
            return true;
        }
    }
}
