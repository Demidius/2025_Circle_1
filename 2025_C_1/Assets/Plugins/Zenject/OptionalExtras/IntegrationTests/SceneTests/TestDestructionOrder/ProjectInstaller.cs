namespace Zenject.Tests.TestDestructionOrder
{
    public class ProjectInstaller : MonoInstaller<ProjectInstaller>
    {
        public override void BaseSceneInstaller()
        {
            Container.BindInterfacesTo<FooDisposable3>().AsSingle();
        }
    }
}
