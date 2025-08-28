using UnityEngine;
namespace CodeBase.System.Services.Utilities.Coroutines.CoroutinRuner
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }

}
