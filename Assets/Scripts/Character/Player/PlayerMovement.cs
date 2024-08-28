using UnityEngine;

namespace Ducksten.ZombieShooterTT.Characters {
    public class PlayerMovement : MonoBehaviour {
        [SerializeField] private Player _player;
        [SerializeField] private Transform _startingPosition;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _cameraRoot;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _cameraAngleRestriction = 70f;

        private Vector2 _moveVector;
        private Vector2 _rotateVector;

        public void Reset() {
            transform.position = _startingPosition.position;
            transform.rotation = _startingPosition.rotation;
            _cameraRoot.localRotation = Quaternion.identity;
        }

        private void OnEnable() {
            if (_player) {
                _player.InputController.onMovementVectorChanged += MovementVectorChanged;
                _player.InputController.OnRotationVectorChanged += RotationVectorChanged;
            }
        }

        private void OnDisable() {
            if (_player) {
                _player.InputController.onMovementVectorChanged -= MovementVectorChanged;
                _player.InputController.OnRotationVectorChanged -= RotationVectorChanged;
            }
        }

        private void FixedUpdate() {
            Move();
        }

        private void Update() {
            Rotate();
        }

        private void Move() {
            var newPosition = transform.position + transform.rotation * new Vector3(_moveVector.x, 0f, _moveVector.y) * Time.fixedDeltaTime;
            _rigidbody.MovePosition(newPosition);
        }

        private void Rotate() {
            var vector = _rotateVector * Time.deltaTime;
            
            transform.Rotate(Vector3.up, vector.x, Space.Self);

            var cameraRotation = _cameraRoot.localEulerAngles;
            var newRotationX = cameraRotation.x - vector.y;
            if (newRotationX >= 180f) {
                newRotationX -= 360f;
            }

            if (Mathf.Abs(newRotationX) > _cameraAngleRestriction) {
                newRotationX = _cameraAngleRestriction * Mathf.Sign(newRotationX);
            }
   
            _cameraRoot.localRotation = Quaternion.Euler(newRotationX, cameraRotation.y, cameraRotation.z);
        }

        private void MovementVectorChanged(Vector2 moveVector) {
            _moveVector = moveVector * _moveSpeed;
        }

        private void RotationVectorChanged(Vector2 rotateVector) {
            _rotateVector = rotateVector * _rotationSpeed;
        }
    }
}
