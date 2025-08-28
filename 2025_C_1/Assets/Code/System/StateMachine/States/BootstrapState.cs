using System.Collections;
using CodeBase._2UIModuleF.UIControllers;
using CodeBase._2UIModuleF.Windows.MenuWindows;
using CodeBase.System.GameSystems.StateMachine.Core;
using CodeBase.System.Services.Utilities.Coroutines.CoroutinRuner;
using UnityEngine;
using Zenject;

namespace CodeBase.System.GameSystems.StateMachine.States
{
    public class BootstrapState : GameState
    {
        private GameStateSwitcher _switcher;
        private ICoroutineRunner _coroutineRunner;
        private WindowProvider _windowProvider;
      


        [Inject]
        void Construct(
            GameStateSwitcher switcher,
            ICoroutineRunner coroutineRunner,
            WindowProvider windowProvider
            )
        {
            _windowProvider = windowProvider;
            _coroutineRunner = coroutineRunner;
            _switcher = switcher;
        }

        public override void Enter()
        {
            _coroutineRunner.StartCoroutine(WaitingAndStart());
        }

        public override void Exit()
        {
            _windowProvider.ResetAllWindows();
            _windowProvider.ShowWindow<WindowMenu>();
        }

        private IEnumerator WaitingAndStart()
        {
            yield return new WaitForSeconds(0.1f);
            _switcher.ToStateMainMenu();
        }
    }
}
