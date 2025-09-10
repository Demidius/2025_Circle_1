using System.ComponentModel;
using Code.GameEntities.Vehicle;
using Code.GameEntyties.Player;
using Code.GameEntyties.Player.Suspension;
using Code.System.Input;
using Code.TODO;
using Code.UIModule.Controllers;
using CodeBase._2UIModuleF.UIControllers;
using CodeBase.System.GameSystems.AudioModule.BaseLogic;
using CodeBase.System.GameSystems.Pools;
using CodeBase.System.GameSystems.Pools.Factory;
using CodeBase.System.Services.Utilities.Coroutines.CoroutinRuner;
using UnityEngine;
using Zenject;
namespace CodeBase._1InstallBindings
{
    public class GameBildings : MonoInstaller {
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GogScript>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<CoroutineRunner>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<WindowProvider>().AsSingle();

            Container.BindInterfacesAndSelfTo<InputModuleController>()
                .AsSingle()
                .NonLazy(); 

            Container.BindInterfacesAndSelfTo<UIModuleContainer>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<AudioManager>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<AudioTracksBase>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<FactoryComponent>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PoolController>().FromComponentsInHierarchy().AsSingle();
            


            //Player
            Container.Bind<ITankTelemetry>().To<TankTelemetryService>().AsSingle();
            Container.Bind<ITankTelemetryReadOnly>().To<TankTelemetryService>().FromResolve();
            Container.Bind<ITankInputProvider>().To<KeyboardTankInputProvider>().AsSingle();
           
        }
    }
}
