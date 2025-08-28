using System;
using UnityEngine;
namespace CodeBase.System.GameSystems.Input
{
    public interface IInputCase
    {
        event Action<Vector2> OnMoveDirection;
        event Action<Vector2> OnMousePoint;
        event Action<Vector2> OnSystemMousePoint;
        event Action OnDistantAttackDown;
        event Action OnDistantAttackUp;
        event Action OnMeleeAttack;
        event Action OnEscapeDown;
        event Action OnTimeSlowBDown;
        event Action OnTimeSlowBUp;

        public void SwitchGameplayState(bool isActive);


    }
}
