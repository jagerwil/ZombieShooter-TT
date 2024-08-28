using UnityEngine;

namespace Ducksten.ZombieShooterTT.Characters {
    public class PlayerAnimationController : MonoBehaviour {
        [SerializeField] private Player _player;
        [SerializeField] private Animator _animator;
        [Header("Animator parameters")]
        [SerializeField] private string _speedX = "SpeedX";
        [SerializeField] private string _speedZ = "SpeedZ";
        [SerializeField] private string _takeDamageTrigger = "TakeDamage";
        [SerializeField] private string _reloadTrigger = "Reload";
        [SerializeField] private string _shootTrigger = "Shoot";
        [Space]
        [SerializeField] private string _deathTrigger = "Death";
        [SerializeField] private string _deathAngle = "DeathAngleY";

        public void Reset() {
            _animator.Rebind();
            _animator.Update(0f);
        }

        private void OnEnable() {
            _player.InputController.onMovementVectorChanged += MovementVectorChanged;
            _player.HealthComponent.onHealthChanged += HealthChanged;
            _player.HealthComponent.onDeath += Death;
            _player.WeaponHandler.onWeaponShot += WeaponShot;
            _player.WeaponHandler.onWeaponReloadStarted += WeaponReloadStarted;
        }

        private void OnDisable() {
            if (!_player) {
                return;
            }

            _player.InputController.onMovementVectorChanged -= MovementVectorChanged;
            _player.HealthComponent.onHealthChanged -= HealthChanged;
            _player.HealthComponent.onDeath -= Death;
            _player.WeaponHandler.onWeaponShot -= WeaponShot;
            _player.WeaponHandler.onWeaponReloadStarted -= WeaponReloadStarted;
        }

        private void MovementVectorChanged(Vector2 vector) {
            _animator.SetFloat(_speedX, vector.x);
            _animator.SetFloat(_speedZ, vector.y);
        }

        private void HealthChanged(int oldHealth, int newHealth) {
            if (oldHealth <= newHealth)
                return;
            
            _animator.SetTrigger(_takeDamageTrigger);
        }

        private void Death(CharacterHealth instigator) {
            var direction = _player.transform.position - instigator.transform.position;
            var angle = Quaternion.LookRotation(direction);
            var angleY = angle.eulerAngles.y;
            if (angleY < 0f) {
                angleY += 360f;
            }
            _animator.SetFloat(_deathAngle, angleY);
            _animator.SetTrigger(_deathTrigger);
        }

        private void WeaponShot(CharacterHealth target) {
            _animator.SetTrigger(_shootTrigger);
        }

        private void WeaponReloadStarted(float duration) {
            _animator.SetTrigger(_reloadTrigger);
        }
    }
}
