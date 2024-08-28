using System;
using System.Collections.Generic;
using Ducksten.Core.ObjectPooling;
using Ducksten.Core.Timer;
using UnityEngine;

namespace Ducksten.ZombieShooterTT.Enemies {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private List<Transform> _startPoints;

        private int _currentStartPointIndex;
        private bool _isActive;
        private RepeatedTimer _timer;
        private Action<Enemy> _onEnemySpawned;

        public void Init(float timeBetweenSpawns, Action<Enemy> onEnemySpawned) {
            if (_timer == null) {
                _timer = TimerManager.Instance.GetTimer();
            }
            else {
                _timer.Stop();
            }

            _timer.Init(timeBetweenSpawns, SpawnEnemy);
            _onEnemySpawned = onEnemySpawned;
        }

        public void SetTimeBetweenSpawns(float timeBetweenSpawns) {
            _timer.SetInterval(timeBetweenSpawns);
        }

        public void StartSpawning() {
            _isActive = true;
            _timer.Start();
        }

        public void StopSpawning() {
            _isActive = false;
        }

        private void OnDestroy() {
            _timer?.Despawn();
        }

        private void SpawnEnemy() {
            if (!_isActive) {
                _timer.Stop();
                return;
            }

            var tf = transform;
            var enemy = PoolObjectFactory.SpawnObject<Enemy>(tf);
            if (!enemy)
                return;

            var startPoint = _startPoints[_currentStartPointIndex];
            var enemyTransform = enemy.transform;
            enemyTransform.position = startPoint.position;
            enemyTransform.localRotation = Quaternion.identity;
            enemy.Reset();
            
            IncreaseStartPointIndex();
            _onEnemySpawned?.Invoke(enemy);
        }

        private void IncreaseStartPointIndex() {
            _currentStartPointIndex += 1;
            if (_currentStartPointIndex >= _startPoints.Count) {
                _currentStartPointIndex = 0;
            }
        }
    }
}
