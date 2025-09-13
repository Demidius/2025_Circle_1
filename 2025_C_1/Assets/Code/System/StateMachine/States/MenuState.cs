using Code.System.Input;
using CodeBase.System.GameSystems.StateMachine.Core;
using UnityEngine;
using Zenject;

namespace CodeBase.System.GameSystems.StateMachine.States
{
    public class MenuState : GameState
    {
        [Inject] private IInputCase _inputCase;
        [Inject] private GameStateSwitcher _switcher;

        public override void Enter()
        {
            CursorUnlock();
                
            _inputCase.SwitchGameplayState(false);
            
            _switcher.ToStateGame();
        }

        public override void Exit()
        {
            // _inputCase.OnEscapeDown -= OnMenu;
        }

        private void OnMenu()
        {
        }
        
        void CursorUnlock()
        {
            Cursor.visible = true;  
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
