using Code.GameEntyties.Shell;
using UnityEngine;
using Zenject;
namespace CodeBase._1InstallBindings
{
    public class GamePoolsInstaller: MonoInstaller
    {
        [SerializeField] private Shell _shellPrefab;
        [SerializeField] private Explosion _expPrefab;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<Shell, Shell.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_shellPrefab)
                .UnderTransformGroup("Shells");
            
            Container.BindMemoryPool<Explosion, Explosion.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_expPrefab)
                .UnderTransformGroup("Explosion");
        }
    }
}
