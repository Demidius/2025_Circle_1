
using UnityEngine.EventSystems;

namespace CodeBase._2UIModuleF.Buttons
{
    public class ButtonSoundHandler : AudioSoursMono, IPointerEnterHandler, IPointerClickHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayHoverSound();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            PlayClickSound();
        }

        private void PlayHoverSound()
        {
            _audioManager.PlaySound(_audioTracksBase.SystemUnderButton, transform.position);
        }

        private void PlayClickSound()
        {
            _audioManager.PlaySound(_audioTracksBase.BottonDownClick, transform.position);
        }
    }
}
