using System.Collections;
using UnityEngine;
namespace CodeBase.System.Services.Utilities.Coroutines.CoroutinRuner
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        
        void StopCoroutine(Coroutine coroutine);
    }
}
