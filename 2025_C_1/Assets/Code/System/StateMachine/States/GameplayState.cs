using Code.System.Input;
using CodeBase.System.GameSystems.StateMachine.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace CodeBase.System.GameSystems.StateMachine.States
{
    public class GameplayState : GameState
    {

        private IInputCase _inputCase;

        [Inject]
        void Construct(
            IInputCase inputCase
            )
        {
            _inputCase = inputCase;
        }

        public override void Enter()
        {
            CursorLock();
            _inputCase.SwitchGameplayState(true);
        }

        public override void Exit()
        {

        }

        
        void CursorLock()
        {
            Cursor.visible = false;  
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
