namespace Zenject.Tests.DecoratorTests
{
    public class Scene1Installer : MonoInstaller<Scene1Installer>
    {
        public override void BaseSceneInstaller()
        {
            Container.Bind<Bar>().AsSingle();
        }
    }
}
