using System;
using Ducksten.ZombieShooterTT.Events;
using UnityEngine;
using CallbackContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace Ducksten.ZombieShooterTT.Characters {
    public class PlayerInputController : MonoBehaviour {
        private PlayerInputActions _input;

        public event Action<Vector2> onMovementVectorChanged;
        public event Action<Vector2> OnRotationVectorChanged;

        private void Awake() {
            _input = new PlayerInputActions();

            _input.ActionMap.Movement.performed += MovementPerformed;
            _input.ActionMap.Movement.canceled += MovementCancelled;
            
            _input.ActionMap.Rotation.performed += RotationPerformed;
            _input.ActionMap.Rotation.canceled += RotationCancelled;
        }

        private void OnEnable() {
            GameEvents.onGameEnded.Subscribe(DisableInput);
            GameEvents.onGameRestarted.Subscribe(EnableInput);
            EnableInput();
        }

        private void OnDisable() {
            GameEvents.onGameEnded.Unsubscribe(DisableInput);
            GameEvents.onGameRestarted.Unsubscribe(EnableInput);
            DisableInput();
        }

        private void DisableInput() {
            _input.Disable();
        }

        private void EnableInput() {
            _input.Enable();
        }

        private void MovementPerformed(CallbackContext ctx) {
            var moveVector = ctx.ReadValue<Vector2>();
            onMovementVectorChanged?.Invoke(moveVector);
        }

        private void MovementCancelled(CallbackContext ctx) {
            onMovementVectorChanged?.Invoke(Vector2.zero);
        }

        private void RotationPerformed(CallbackContext ctx) {
            var rotateVector = ctx.ReadValue<Vector2>();
            OnRotationVectorChanged?.Invoke(rotateVector);
        }

        private void RotationCancelled(CallbackContext ctx) {
            OnRotationVectorChanged?.Invoke(Vector2.zero);
        }
    }
}