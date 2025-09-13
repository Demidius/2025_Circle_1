using UnityEngine;

#pragma warning disable 649

namespace Zenject.Tests.Bindings.FromSubContainerPrefab
{
    public class FooInstaller : MonoInstaller
    {
        [SerializeField]
        Bar _bar;

        public override void BaseSceneInstaller()
        {
            Container.BindInstance(_bar);
            Container.Bind<Gorp>().WithId("gorp").AsSingle();
        }
    }
}
