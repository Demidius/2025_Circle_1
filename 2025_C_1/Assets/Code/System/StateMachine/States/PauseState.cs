using CodeBase.System.GameSystems.Input;
using CodeBase.System.GameSystems.StateMachine.Core;
using Zenject;

namespace CodeBase.System.GameSystems.StateMachine.States
{
    public class PauseState : GameState
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
            _inputCase.SwitchGameplayState(false);
        }
    
        public override void Exit()
        {
            _inputCase.SwitchGameplayState(true);
        }
      
    }
}

