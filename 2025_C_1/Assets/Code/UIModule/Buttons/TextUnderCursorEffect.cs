using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase._2UIModuleF.Buttons
{
    public class TextUnderCursorEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] Image _underCursorGround;
        [SerializeField] TextMeshProUGUI _underCursorText;
        private Color _originalColor;


        private void Start()
        {
            _originalColor = _underCursorText.color;

            DOTween.Sequence()
                .Join(_underCursorGround.DOFade(0f, 0f));

        }

        private void OnEnable()
        {
            DOTween.Sequence()
                .Join(_underCursorGround.DOFade(0f, 0f));

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            DOTween.Sequence()
                .Join(_underCursorGround.DOFade(1f, 0.2f))  
                .Join(_underCursorText.DOColor(Color.black, 0.2f)); 
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            DOTween.Sequence()
                .Join(_underCursorGround.DOFade(0f, 0.2f))  
                .Join(_underCursorText.DOColor(_originalColor, 0.2f)); 
      
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            DOTween.Sequence()
                .Join(_underCursorText.DOColor(Color.gray, 0.2f)); 
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            DOTween.Sequence()
                .Join(_underCursorText.DOColor(_originalColor, 0.2f)); 
        }
    }
}
