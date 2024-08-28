using UnityEngine;

namespace Ducksten.ZombieShooterTT.Enemies {
    public class EnemyAnimationController : MonoBehaviour {
        [SerializeField] private Enemy _enemy;
        [SerializeField] private Animator _animator;
        [Header("Animator parameters")]
        [SerializeField] private string _attackTrigger = "Attack";
        [SerializeField] private string _isMovingBool = "IsMoving";
        [SerializeField] private string _takeDamageTrigger = "TakeDamage";
        [SerializeField] private string _deathTrigger = "Death";
        //[SerializeField] private AnimationIdsEnemy _enemyIds;

        public void Reset() {
            _animator.Rebind();
            _animator.Update(0f);
        }

        private void OnEnable() {
            _enemy.HealthComponent.onHealthChanged += HealthChanged;
            _enemy.HealthComponent.onDeath += Death;
            _enemy.AttackBehaviour.onAttackStarted += AttackStarted;
            _enemy.Pathfinding.onStartedMoving += StartedMoving;
            _enemy.Pathfinding.onStoppedMoving += StoppedMoving;
        }

        private void OnDisable() {
            if (_enemy) {
                _enemy.HealthComponent.onHealthChanged -= HealthChanged;
                _enemy.HealthComponent.onDeath -= Death;
                _enemy.AttackBehaviour.onAttackStarted -= AttackStarted;
                _enemy.Pathfinding.onStartedMoving -= StartedMoving;
                _enemy.Pathfinding.onStoppedMoving -= StoppedMoving;
            }
        }

        private void HealthChanged(int oldHealth, int newHealth) {
            if (oldHealth <= newHealth) {
                return;
            }
            
            _animator.SetTrigger(_takeDamageTrigger);
        }

        private void Death(CharacterHealth instigator) {
            _animator.SetTrigger(_deathTrigger);
        }

        private void StartedMoving() {
            _animator.SetBool(_isMovingBool, true);
        }

        private void StoppedMoving() {
            _animator.SetBool(_isMovingBool, false);
        }

        private void AttackStarted() {
            _animator.SetTrigger(_attackTrigger);
        }

        private void Start() {
            /*
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //entityManager.AddComponentObject();
            var entityQuery = entityManager.CreateEntityQuery(typeof(GpuEcsAnimatorControlComponent));
            var entities = entityQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities) {
                //entity.
            }
            */
            
        }
    }
}
