using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace CodeBase._2UIModuleF.Mouse
{
    public class CursorSliderPointer : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {

        [SerializeField] private UICursor _uiCursor;

        public void OnPointerDown(PointerEventData eventData)
        {
            _uiCursor.MouseChange(MouseType.GameMode);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _uiCursor.MouseChange(MouseType.MenuMode);
        }

    }
}
 