namespace Zenject.Tests.Installers.MonoInstallers
{
    public class Foo
    {
    }

    public class FooInstaller : MonoInstaller<FooInstaller>
    {
        public override void BaseSceneInstaller()
        {
            Container.Bind<Foo>().AsSingle().NonLazy();
        }
    }
}
