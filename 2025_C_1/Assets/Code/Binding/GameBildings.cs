using System.ComponentModel;
using Code.UIModule.Controllers;
using CodeBase._2UIModuleF.UIControllers;
using CodeBase.OnWork;
using CodeBase.System.GameSystems.AudioModule.BaseLogic;
using CodeBase.System.GameSystems.Input;
using CodeBase.System.Services.Utilities.Coroutines.CoroutinRuner;
using Zenject;
namespace CodeBase._1InstallBindings
{
    public class GameBildings : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GogScript>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<CoroutineRunner>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<WindowProvider>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<InputModuleController>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<UIModuleContainer>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<AudioManager>().FromComponentsInHierarchy().AsSingle();
           
            Container.BindInterfacesAndSelfTo<AudioTracksBase>().FromComponentsInHierarchy().AsSingle();
            
            



        }
    }
}
