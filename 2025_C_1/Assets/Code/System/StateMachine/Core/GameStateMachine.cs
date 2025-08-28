using System;
using System.Collections.Generic;
using CodeBase.System.GameSystems.StateMachine.States;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace CodeBase.System.GameSystems.StateMachine.Core
{
    public class GameStateMachine : IInitializable
    {
        // Текущее активное состояние
        private GameState _activeState;

        // Словарь со всеми доступными состояниями
        private Dictionary<Type, GameState> _states = new Dictionary<Type, GameState>();
        public event Action<GameState> OnStateChanged;

        [Inject]
        private void Construct(
            BootstrapState bootstrapState,
            MenuState menuState,
            GameplayState gameplayState,
            PauseState pauseState,
            GameOverState gameOverState,
            DieState dieState)
        {
            RegisterState(bootstrapState);
            RegisterState(menuState);
            RegisterState(gameplayState);
            RegisterState(pauseState);
            RegisterState(gameOverState);
            RegisterState(dieState);
            
            ChangeState<BootstrapState>();
        }
      
        public void RegisterState(GameState state)
        {
            _states[state.GetType()] = state;
        }
                                                                                                                                                               
        public void ChangeState<T>() where T : GameState
        {
            // Debug.Log("Game State last is " + _activeState?.ToString());
            _activeState?.Exit();
            Type stateType = typeof(T);
            if (_states.TryGetValue(stateType, out GameState newState))
            {
                _activeState = newState;
              
                _activeState.Enter();
                OnStateChanged?.Invoke(_activeState);
                Debug.Log("Game State Changed on " + stateType.ToString());
              
            }
            else
                Debug.LogError($"Состояние {stateType.Name} не зарегистрировано!");
        }

       
        public T GetCurrentState<T>() where T : GameState => _activeState as T;

        public  bool IsInState<T>() where T : GameState => _activeState is T;

        public void Initialize()
        {
           
        }
    }
}
