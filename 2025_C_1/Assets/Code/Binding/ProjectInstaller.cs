using Code.UIModule.Controllers;
using CodeBase.System.GameSystems.AudioModule.BaseLogic;
using CodeBase.System.GameSystems.Pools;
using CodeBase.System.GameSystems.Pools.Factory;
using UnityEngine;
using Zenject;

namespace Code.Binding
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] PoolController poolPrefab;
        public override void BaseSceneInstaller()
        {
            Container.BindInterfacesAndSelfTo<AudioManager>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<AudioTracksBase>().FromComponentsInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<FactoryComponent>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<PoolController>().FromComponentsInHierarchy().AsSingle();
        }
    }

}
