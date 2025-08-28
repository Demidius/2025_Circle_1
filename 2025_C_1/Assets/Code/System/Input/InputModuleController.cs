using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CodeBase.System.GameSystems.Input
{
    public class InputModuleController : IInputCase, IDisposable
    {
        private InputModule _inputModule;
        public event Action<Vector2> OnMoveDirection;
        public event Action<Vector2> OnMousePoint;
        public event Action<Vector2> OnSystemMousePoint;
        public event Action OnDistantAttackDown;
        public event Action OnDistantAttackUp;
        public event Action OnMeleeAttack;
        public event Action OnEscapeDown;
        public event Action OnTimeSlowBDown;
        public event Action OnTimeSlowBUp;

        // Конструктор может использоваться напрямую или через Zenject
        public InputModuleController()
        {
            _inputModule = new InputModule();
            _inputModule.Enable();
            
            _inputModule.Player.Move.performed += OnMove;
           
        }


        public void SwitchGameplayState(bool isActive)
        {

            if (isActive)
                _inputModule.Player.Enable();
            else
                _inputModule.Player.Disable();
        }

        private void OnEscape(InputAction.CallbackContext obj)
        {
            // Debug.Log("Escape");
            OnEscapeDown?.Invoke();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();

            OnMoveDirection?.Invoke(direction);
        }

        private void OnMousePos(InputAction.CallbackContext context)
        {
            Vector2 position = context.ReadValue<Vector2>();
            OnMousePoint?.Invoke(position);
        }
        private void OnSystemMousePos(InputAction.CallbackContext context)
        {
            Vector2 position = context.ReadValue<Vector2>();
            OnSystemMousePoint?.Invoke(position);
        }

        private void OnDistansDown(InputAction.CallbackContext context)
        {
            OnDistantAttackDown?.Invoke();
        }

        private void OnDistansUp(InputAction.CallbackContext context)
        {
            OnDistantAttackUp?.Invoke();
        }

        private void OnMelee(InputAction.CallbackContext context)
        {
            OnMeleeAttack?.Invoke();
        }

        private void OnSlowTime(InputAction.CallbackContext context)
        {
            OnTimeSlowBDown?.Invoke();
        }

        private void OffSlowTime(InputAction.CallbackContext obj)
        {
            OnTimeSlowBUp?.Invoke();
        }

        public void Dispose()
        {
            // Отписываемся от событий, чтобы избежать утечек памяти
            _inputModule.Player.Move.performed -= OnMove;
           

            // Отключаем и освобождаем ресурсы InputModule
            _inputModule.Disable();
            _inputModule.Dispose();
        }




    }

}
