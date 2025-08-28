using UnityEngine;
namespace CodeBase.System.Services.Utilities
{
    public class CursorToSprite : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorSprite; // Текстура для курсора
        [SerializeField] private Vector2 hotSpot = Vector2.zero; // Точка привязки (по умолчанию верхний левый угол)

        private void Start()
        {
            // Установить кастомный курсор
            Cursor.SetCursor(cursorSprite, hotSpot, CursorMode.Auto);
        }

        private void OnDisable()
        {
            // Вернуть стандартный курсор, если объект деактивирован
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}