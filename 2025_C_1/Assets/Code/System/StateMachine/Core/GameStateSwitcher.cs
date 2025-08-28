using CodeBase.System.GameSystems.StateMachine.States;
using Zenject;
namespace CodeBase.System.GameSystems.StateMachine.Core
{
    public class GameStateSwitcher
    {
        private GameStateMachine _stateMachine;

        [Inject]
        private void Construct(GameStateMachine machine)
        {
            _stateMachine = machine;
        }

        public void ToStateBootstrap() => _stateMachine.ChangeState<BootstrapState>();
        public void ToStateMainMenu()  => _stateMachine.ChangeState<MenuState>();
        public void ToStateGame()      => _stateMachine.ChangeState<GameplayState>();
        public void ToStatePause()     => _stateMachine.ChangeState<PauseState>();
        public void ToStateEnd()       => _stateMachine.ChangeState<GameOverState>();
        public void ToStateDie()       => _stateMachine.ChangeState<DieState>();

        public bool IsPlaying()     => _stateMachine.IsInState<GameplayState>();
        public bool IsPaused()      => _stateMachine.IsInState<PauseState>();
        public bool IsInMenu()      => _stateMachine.IsInState<MenuState>();
        public bool IsGameOver()    => _stateMachine.IsInState<GameOverState>();
        public bool IsInBootstrap() => _stateMachine.IsInState<BootstrapState>();
        public bool IsDie() => _stateMachine.IsInState<DieState>();

        public string GetCurrentStateName()
        {
            var state = _stateMachine.GetCurrentState<GameState>();
            return state != null ? state.GetType().Name : "Нет активного состояния";
        }
    }
}
