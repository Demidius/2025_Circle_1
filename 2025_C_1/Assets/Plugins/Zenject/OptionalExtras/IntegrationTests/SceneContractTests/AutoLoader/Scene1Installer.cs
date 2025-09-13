namespace Zenject.Tests.AutoLoadSceneTests
{
    public class Scene1Installer : MonoInstaller<Scene1Installer>
    {
        public override void BaseSceneInstaller()
        {
            Container.Bind<Qux>().AsSingle();
        }
    }
}
