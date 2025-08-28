using CodeBase.System.GameSystems.Input;
using CodeBase.System.GameSystems.StateMachine.Core;
using Zenject;

namespace CodeBase.System.GameSystems.StateMachine.States
{
    public class DieState : GameState
    {
        [Inject] private IInputCase _inputCase;

        public override void Enter()
        {
            _inputCase.OnEscapeDown += OnMenu;

            _inputCase.SwitchGameplayState(false);
        }

        public override void Exit()
        {
            // Debug.Log("Exit");
            _inputCase.OnEscapeDown -= OnMenu;
        }

        private void OnMenu()
        {
         
        }
    }
}
