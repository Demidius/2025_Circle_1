using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.System.Input
{
    // Контроллер ввода: создаёт InputMain, включает карту и раздаёт события наружу.
    public class InputModuleController : IInputCase, IDisposable
    {
        private readonly InputModule _input;

        public event Action<Vector2> OnMoveDirection;
        // public event Action<Vector2> OnMousePoint;
        // public event Action<Vector2> OnSystemMousePoint;
        // public event Action OnDistantAttackDown;
        // public event Action OnDistantAttackUp;
        // public event Action OnMeleeAttack;
        // public event Action OnEscapeDown;
        // public event Action OnTimeSlowBDown;
        // public event Action OnTimeSlowBUp;

        public InputModuleController()
        {
            _input = new InputModule();
            _input.Enable();
            _input.Player.Enable();

            // Подписываемся на все фазы, чтобы ничего не потерять
            _input.Player.Move.started   += OnMove;
            _input.Player.Move.performed += OnMove;
            _input.Player.Move.canceled  += OnMove;

            Debug.Log($"[Input] Game Input Module Enabled. Move initial={_input.Player.Move.ReadValue<Vector2>()}");
        }

        private void OnMove(InputAction.CallbackContext ctx)
        {
            var v = ctx.ReadValue<Vector2>();
            // Debug.Log($"[Input] Move {ctx.phase}: {v}");
            OnMoveDirection?.Invoke(v);
        }

        public void SwitchGameplayState(bool isActive)
        {
            if (isActive) _input.Player.Enable();
            else _input.Player.Disable();
        }

        public void Dispose()
        {
            _input.Player.Move.started   -= OnMove;
            _input.Player.Move.performed -= OnMove;
            _input.Player.Move.canceled  -= OnMove;

            _input.Disable();
            _input.Dispose();
        }
    }
}
