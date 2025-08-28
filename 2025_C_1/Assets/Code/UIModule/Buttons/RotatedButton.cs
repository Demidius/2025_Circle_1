using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CodeBase._2UIModuleF.Buttons
{
    public class RotatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {


        [Tooltip("Длительность анимации в секундах")]
        public float duration = 0.5f;

        // Храним ссылку на текущее твин
        private Tweener rotateTween;

        private GameObject _rotatedButton;

        private void Start()
        {
            _rotatedButton = transform.GameObject();
        }
        // При наведении
        public void OnPointerEnter(PointerEventData eventData)
        {
            // Остановить предыдущую анимацию, если она ещё играет
            rotateTween?.Kill();
            // Запустить поворот на 180° по оси Y
            rotateTween = transform.DORotate(new Vector3(0, 0, 180), duration)
                .SetEase(Ease.OutBack);
        }

        // При уведении
        public void OnPointerExit(PointerEventData eventData)
        {
            rotateTween?.Kill();
            // Вернуть в исходное положение (0,0,0)
            rotateTween = transform.DORotate(Vector3.zero, duration)
                .SetEase(Ease.OutBack);
        }
    }
}
