using System;
using Ducksten.Utils;
using Ducksten.ZombieShooterTT.Events;
using UnityEngine;

namespace Ducksten.ZombieShooterTT.Characters {
    public class PlayerWeaponHandler : MonoBehaviour {
        [SerializeField] private Player _player;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private float _maxAimDistance = 20f;
        [SerializeField] private float _aimAssistSmallRadius = 0.05f;
        [SerializeField] private float _aimAssistBigRadius = 3f;
        [SerializeField] private float _raycastStep = 0.3f;

        private CharacterHealth _target;
        private bool _shouldCheckTargets;

        public int BulletCount => _weapon.BulletCount;
        public int MaxBulletCount => _weapon.MaxBulletCount;

        //Made external events in case if there would be weapon changing logic later down the line
        public event Weapon.WeaponShotDelegate onWeaponShot;
        public event Weapon.WeaponReloadStartedDelegate onWeaponReloadStarted;
        public event Action onWeaponReloadEnded;

        public void Reset() {
            _weapon.Reset();
            _shouldCheckTargets = true;
        }

        private void Start() {
            _weapon.Init(_player.HealthComponent, (target) => onWeaponShot?.Invoke(target), 
                         (duration) => onWeaponReloadStarted?.Invoke(duration), 
                         () => onWeaponReloadEnded?.Invoke());
        }

        private void OnEnable() {
            GameEvents.onGameEnded.Subscribe(StopAutoShooting);
        }

        private void OnDisable() {
            GameEvents.onGameEnded.Unsubscribe(StopAutoShooting);
        }

        private void Update() {
            if (!_shouldCheckTargets)
                return;
            
            //var targetTransform = PhysicsUtils.GetClosestConeCast(_cameraTransform, _cameraTransform.forward, _maxAimDistance, _aimAssistConeAngle, _enemyLayer);
            var targetTransform = PhysicsUtils.GetClosestRaycastCylinder(_cameraTransform, _cameraTransform.forward,
                _maxAimDistance, _aimAssistSmallRadius, _aimAssistBigRadius, _raycastStep, _enemyLayer);

            var target = targetTransform ? targetTransform.GetComponent<CharacterHealth>() : null;
            if (_target == target)
                return;

            _target = target;
            if (_target) {
                _weapon.TargetSpotted(_target);
                return;
            }
            
            _weapon.TargetLost();
        }

        private void StopAutoShooting() {
            _weapon.StopAutoShooting();
            _shouldCheckTargets = false;
        }
    }
}

