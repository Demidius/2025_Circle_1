
    using System;
    using UnityEngine;
    using DG.Tweening; // обязательно подключи DOTween

    namespace Code.GameEntyties.Player.Turret
    {
        public class ShootingFireEffect : MonoBehaviour
        {
            [SerializeField] private float _duration = 0.2f; // время пока видно эффект

            private void OnEnable()
            {
                // Отменяем возможные старые твины
                DOTween.Kill(this);

                // Ждём и отключаем объект
                DOVirtual.DelayedCall(_duration, () =>
                {
                    gameObject.SetActive(false);
                }).SetId(this);
            }
        }
    }

