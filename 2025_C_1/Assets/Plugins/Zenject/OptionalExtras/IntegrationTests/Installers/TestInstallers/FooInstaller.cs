namespace Zenject.Tests.Installers.Installers
{
    public class Foo
    {
    }

    public class FooInstaller : Installer<FooInstaller>
    {
        public override void BaseSceneInstaller()
        {
            Container.Bind<Foo>().AsSingle().NonLazy();
        }
    }
}
