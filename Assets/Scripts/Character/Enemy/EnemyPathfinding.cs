using System;
using Ducksten.ZombieShooterTT.Characters;
using Pathfinding;
using UnityEngine;

namespace Ducksten.ZombieShooterTT.Enemies {
    public class EnemyPathfinding : MonoBehaviour {
        [SerializeField] private AIPath _path;
        [SerializeField] private AIDestinationSetter _destinationSetter;
        [SerializeField] private float _moveSpeed;

        private bool _isMoving;
        private bool _isEnemyClose;

        public event Action onStartedMoving;
        public event Action onStoppedMoving;
        public event Action onReachedPlayer;

        public void Reset() {
            _destinationSetter.target = Player.Instance.transform;
            _path.maxSpeed = _moveSpeed;
            onStartedMoving?.Invoke();
            ResumePathfinding();
            _isMoving = false;
        }

        private void Update() {
            if (!_path.enabled) {
                RotateTowardsTarget();
                return;
            }
            
            var isMoving = !_path.reachedDestination;
            if (isMoving != _isMoving) {
                if (isMoving)
                    onStartedMoving?.Invoke();
                else {
                    onStoppedMoving?.Invoke();
                    if (!_path.pathPending) {
                        onReachedPlayer?.Invoke();
                    }
                }
            }
            _isMoving = isMoving;
            if (!_isMoving)
                RotateTowardsTarget();
        }

        public void PausePathfinding() {
            _path.enabled = false;
        }

        public void ResumePathfinding() {
            _path.enabled = true;
        }

        private void RotateTowardsTarget() {
            var targetPos = _destinationSetter.target.position;
            transform.rotation = Quaternion.LookRotation(targetPos - transform.position, transform.up);
        }
    }
}
