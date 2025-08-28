using CodeBase.System.GameSystems.StateMachine.Core;
using CodeBase.System.GameSystems.StateMachine.States;
using CodeBase.System.Services.Addressables;
using Zenject;

namespace CodeBase._1InstallBindings
{
    public class GameStateInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SimpleSceneLoader>().AsSingle();

            // Регистрируем контроллер состояний
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
            
            // Регистрируем все состояния

            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<MenuState>().AsSingle();
            Container.Bind<GameplayState>().AsSingle();
            Container.Bind<PauseState>().AsSingle();
            Container.Bind<GameOverState>().AsSingle();
            Container.Bind<DieState>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameStateSwitcher>().AsSingle();
        }
    }



}
