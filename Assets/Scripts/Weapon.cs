using System;
using Ducksten.Core.Timer;
using UnityEngine;

namespace Ducksten.ZombieShooterTT {
    public class Weapon : MonoBehaviour {
        [SerializeField] private int _baseDamage = 10;
        [SerializeField] private float _headshotMultiplier = 2f;
        [SerializeField] private float _bulletsPerSecond = 5f;
        [SerializeField] private int _bulletsInClip = 30;
        [SerializeField] private float _reloadDuration = 1f;

        public delegate void WeaponShotDelegate(CharacterHealth target);
        public delegate void WeaponReloadStartedDelegate(float duration);

        private CharacterHealth _owner;
        private WeaponShotDelegate _shotCallback;
        private WeaponReloadStartedDelegate _reloadStartedCallback;
        private Action _reloadEndedCallback;

        private CharacterHealth _target;
        private RepeatedTimer _timer;
        private int _bulletsLeft;
        private float _timeBetweenBullets;
        private bool _startedReloading;

        public int BulletCount => _bulletsLeft;
        public int MaxBulletCount => _bulletsInClip;

        private void Awake() {
            _timeBetweenBullets = 1f / _bulletsPerSecond;
            _bulletsLeft = _bulletsInClip;
        }

        public void Init(CharacterHealth owner, WeaponShotDelegate weaponShot, WeaponReloadStartedDelegate reloadStarted, Action reloadEnded) {
            _owner = owner;
            _shotCallback = weaponShot;
            _reloadStartedCallback = reloadStarted;
            _reloadEndedCallback = reloadEnded;
            Reset();
        }

        public void Reset() {
            if (_timer == null) {
                _timer = TimerManager.Instance.GetTimer();
            }

            _startedReloading = false;
            _bulletsLeft = _bulletsInClip;
            _reloadEndedCallback?.Invoke();
            _timer.Stop();
            _timer.Init(_timeBetweenBullets, ShootTarget);
            _timer.Start();
        }

        public void StopAutoShooting() {
            _timer.Stop();
        }

        private void OnDestroy() {
            _timer?.Despawn();
        }

        public void TargetSpotted(CharacterHealth target) {
            _target = target;
            _timer.Start();
        }

        public void TargetLost() {
            _target = null;
        }

        private void ShootTarget() {
            if (_bulletsLeft <= 0) {
                if (!_startedReloading) {
                    _timer.SetInterval(_reloadDuration);
                    _startedReloading = true;
                    _reloadStartedCallback?.Invoke(_reloadDuration);
                    return;
                }
                _timer.SetInterval(_timeBetweenBullets);
                _bulletsLeft = _bulletsInClip;
                _startedReloading = false;
                _reloadEndedCallback?.Invoke();
                return;
            }
            
            if (!_target) {
                _timer.Stop();
                return;
            }

            _target.TakeDamage(_owner, _baseDamage);
            _bulletsLeft -= 1;
            _shotCallback?.Invoke(_target);
        }
    }
}

