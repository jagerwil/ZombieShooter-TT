using System;
using System.Collections;
using Ducksten.ZombieShooterTT.Characters;
using TMPro;
using UnityEngine;

namespace Ducksten.ZombieShooterTT.Ui {
    public class PlayerAmmoView : MonoBehaviour {
        [SerializeField] private TMP_Text _text;

        private void OnEnable() {
            var weaponHandler = Player.Instance.WeaponHandler;
            weaponHandler.onWeaponShot += WeaponShot;
            weaponHandler.onWeaponReloadEnded += ReloadEnded;
        }

        private void OnDisable() {
            var player = Player.Instance;
            if (!player)
                return;
            
            player.WeaponHandler.onWeaponShot -= WeaponShot;
            player.WeaponHandler.onWeaponReloadEnded -= ReloadEnded;
        }

        private void Start() {
            StartCoroutine(UpdateTextNextFrame());
        }

        private void WeaponShot(CharacterHealth target) {
            UpdateText();
        }

        private void ReloadEnded() {
            UpdateText();
        }

        private IEnumerator UpdateTextNextFrame() {
            yield return new WaitForEndOfFrame();
            UpdateText();
        }

        private void UpdateText() {
            var weaponHandler = Player.Instance.WeaponHandler;
            var bulletCount = weaponHandler.BulletCount;
            var maxBulletCount = weaponHandler.MaxBulletCount;
            _text.text = $"{bulletCount}/{maxBulletCount}";
        }
    }
}
