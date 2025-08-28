using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace CodeBase.System.Services.Addressables
{
    public class SimpleSceneLoader
    {
        private AsyncOperationHandle<SceneInstance>? _currentHandle;

        public async Task LoadAsync(string sceneKey)
        {
            await UnloadAsync();
            _currentHandle = UnityEngine.AddressableAssets.Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive);
            await _currentHandle.Value.Task;
        }

        public async Task UnloadAsync()
        {
            if (_currentHandle.HasValue)
            {
                // выгружаем сцену и сразу освобождаем handle
                var unload = UnityEngine.AddressableAssets.Addressables.UnloadSceneAsync(_currentHandle.Value, true);
                await unload.Task;
                _currentHandle = null;
            }
        }
    }
}
