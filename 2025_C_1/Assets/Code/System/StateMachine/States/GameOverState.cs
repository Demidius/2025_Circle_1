using CodeBase.System.GameSystems.StateMachine.Core;
using UnityEngine;
namespace CodeBase.System.GameSystems.StateMachine.States
{
    public class GameOverState : GameState
    {
        public override void Enter()
        {
            Debug.Log("Игра окончена");
        }

        public override void Exit()
        {
            Debug.Log("Выход из экрана окончания игры");
        }
    }
}
