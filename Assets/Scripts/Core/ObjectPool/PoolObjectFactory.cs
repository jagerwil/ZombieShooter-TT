using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Ducksten.Core.ObjectPooling {
    public class PoolObjectFactory : MonoBehaviour {
        [SerializeField] private List<PoolObject> _prefabs; //Even better would be to add initial spawn amount

        private static Dictionary<Type, ObjectPool> _pools = new();

        private void Awake() {
            foreach (var prefab in _prefabs) {
                var type = prefab.GetType();
                if (_pools.ContainsKey(type))
                    return;

                var pool = new ObjectPool(prefab, transform);
                _pools.TryAdd(type, pool);
            }
        }

        [CanBeNull]
        public static T SpawnObject<T>(Transform parent = null) where T : PoolObject {
            var pool = GetPool<T>();
            return (T)pool?.SpawnObject(parent);
        }

        public static void DespawnAll<T>() where T : PoolObject {
            var pool = GetPool<T>();
            pool?.DespawnAll();
        }

        private static ObjectPool GetPool<T>() where T : PoolObject {
            var type = typeof(T);
            if (!_pools.TryGetValue(type, out var pool)) {
                Debug.LogError($"{nameof(PoolObjectFactory)}.{nameof(SpawnObject)}(): There is no pool for type {type}");
                return null;
            }
            return pool;
        }
    }
}
