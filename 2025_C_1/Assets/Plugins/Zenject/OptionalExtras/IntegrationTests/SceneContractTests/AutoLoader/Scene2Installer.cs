namespace Zenject.Tests.AutoLoadSceneTests
{
    public class Scene2Installer : MonoInstaller<Scene2Installer>
    {
        public override void BaseSceneInstaller()
        {
            Container.Bind<Bar>().AsSingle();
        }
    }
}
