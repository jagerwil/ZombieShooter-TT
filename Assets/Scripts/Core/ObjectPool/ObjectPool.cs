using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ducksten.Core.ObjectPooling {
    public class ObjectPool {
        private readonly PoolObject _prefab;
        private readonly Transform _objectsRoot;
        private readonly Queue<PoolObject> _freeObjects = new();
        private readonly List<PoolObject> _allObjects = new();

        public ObjectPool(PoolObject prefab, Transform objectsRoot) {
            _prefab = prefab;
            _objectsRoot = objectsRoot;
        }

        public PoolObject SpawnObject(Transform parent = null) {
            PoolObject obj;
            if (_freeObjects.Any()) {
                obj = _freeObjects.Dequeue();
                obj.Spawn(this);
                if (parent)
                    obj.transform.SetParent(parent);
                return obj;
            }

            obj = Object.Instantiate(_prefab, parent ? parent : _objectsRoot);
            obj.Spawn(this);
            _allObjects.Add(obj);
            return obj;
        }

        public void DespawnAll() {
            foreach (var obj in _allObjects) {
                obj.Despawn();
            }
        }

        public void ReturnToPool(PoolObject obj) {
            obj.transform.SetParent(_objectsRoot);
            _freeObjects.Enqueue(obj);
        }
    }
}
