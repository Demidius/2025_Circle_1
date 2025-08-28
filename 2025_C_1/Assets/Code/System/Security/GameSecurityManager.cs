using UnityEngine;
using Zenject;

namespace CodeBase.System.Services.Utilities
{
    public class GameSecurityManager : MonoBehaviour
    {
       
        private void Awake()
        {
            SecureInt.CheatDetected   += OnCheat;
            SecureFloat.CheatDetected += OnCheat;
      
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            // Важно отписаться, чтобы не цеплять «висячие» обработчики
            SecureInt.CheatDetected   -= OnCheat;
            SecureFloat.CheatDetected -= OnCheat;
        }

        private void OnCheat()
        {
            Debug.LogWarning("Cheat detected! Executing punishment…");

         

            // 3. (опционально) записываем в сохранение «флаг читера»,
            //    блокируем ачивки, отключаем мультиплеер и т. д.
        }
    }
}
