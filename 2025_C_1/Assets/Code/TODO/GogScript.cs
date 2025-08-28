// Убедитесь, что это пространство имен AliveEnemiesTracker

using CodeBase.System.Core.Consts;
using CodeBase.System.Services.Addressables;
using UnityEngine;
using Zenject;


namespace CodeBase.OnWork
{
    public class GogScript : MonoBehaviour
    {
     
        [Inject] private SimpleSceneLoader _simpleSceneLoader;

        private void Start()
        {
            _simpleSceneLoader.LoadAsync(Const.GameScene);
        }
        
        // private void Update()
        // {
        //     if (Input.GetKeyDown(KeyCode.Alpha1))
        //     {
        //         _simpleSceneLoader.LoadAsync(Const.GameScene);
        //     }
        // }
    }
}
