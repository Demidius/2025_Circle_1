using Code.Common.Async;
using Code.GameEntities.Vehicle;
using Code.System.Input;
using Code.TODO;
using Code.UIModule.Controllers;
using CodeBase._2UIModuleF.UIControllers;
using CodeBase.System.GameSystems.AudioModule.BaseLogic;
using CodeBase.System.GameSystems.Pools;
using CodeBase.System.GameSystems.Pools.Factory;
using CodeBase.System.Services.Utilities.Coroutines.CoroutinRuner;

using Zenject;
namespace CodeBase._1InstallBindings
{
    public class GameBildings : MonoInstaller {
        
        public override void BaseSceneInstaller()
        {
            Container.BindInterfacesAndSelfTo<GogScript>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<CoroutineRunner>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<WindowProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<InputModuleController>()
                .AsSingle()
                .NonLazy(); 

            Container.BindInterfacesAndSelfTo<UIModuleContainer>().FromComponentsInHierarchy().AsSingle();

         
           
            Container.BindInterfacesAndSelfTo<ReactiveWaiter>().AsSingle();
            
            //Player
            Container.BindInterfacesAndSelfTo<TankTelemetryService>().AsSingle();
            Container.Bind<ITankInputProvider>().To<KeyboardTankInputProvider>().AsSingle();
           
        }
    }
}
