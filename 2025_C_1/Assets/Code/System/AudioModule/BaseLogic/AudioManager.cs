using System.Collections;
using System.Collections.Generic;
using CodeBase.System.GameSystems.AudioModule.BaseLogic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Code.UIModule.Controllers
{
    public class AudioManager : MonoBehaviour
    {
        private Dictionary<FMOD.GUID, SoundPool> _soundPools = new Dictionary<FMOD.GUID, SoundPool>();

        // Инициализация пула для звука
        public SoundPool InitializeSoundPool(EventReference soundPath, int initialCount = 5)
        {
            if (soundPath.Guid.IsNull)
            {
                Debug.LogWarning("Sound reference is empty!");
                return null;
            }

            if (!_soundPools.ContainsKey(soundPath.Guid))
            {
                _soundPools[soundPath.Guid] = new SoundPool(soundPath, initialCount);
            }

            return _soundPools[soundPath.Guid];
        }

        // Воспроизведение звука
        public void PlaySound(EventReference soundPath, Vector3? position = null, bool useInstance = false)
        {
            if (soundPath.Guid.IsNull)
            {
                Debug.LogWarning("Sound reference is empty!");
                return;
            }

            if (!useInstance)
            {
                // Воспроизведение одиночного звука
                if (position.HasValue)
                    RuntimeManager.PlayOneShot(soundPath, position.Value);
                else
                    RuntimeManager.PlayOneShot(soundPath);
            }
            else
            {
                // Использование звукового пула
                if (!_soundPools.ContainsKey(soundPath.Guid))
                {
                    Debug.LogWarning($"Sound pool for {soundPath.Guid} not found. Initializing new pool.");
                    InitializeSoundPool(soundPath);
                }

                var instance = _soundPools[soundPath.Guid].GetInstance();

                // Устанавливаем 3D-атрибуты
                if (position.HasValue)
                    instance.set3DAttributes(RuntimeUtils.To3DAttributes(position.Value));
                else
                    instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

                instance.start();

                // Возвращаем в пул после завершения
                StartCoroutine(ReturnToPoolAfterPlay(instance, soundPath.Guid));
            }
        }

        // Остановка звука по пути
        public void StopSound(EventReference soundPath)
        {
            if (soundPath.Guid.IsNull)
            {
                Debug.LogWarning("Sound reference is empty!");
                return;
            }

            if (_soundPools.ContainsKey(soundPath.Guid))
            {
                _soundPools[soundPath.Guid].StopAll();
            }
            else
            {
                Debug.LogWarning($"Sound pool for {soundPath.Guid} not found.");
            }
        }

        // Остановка всех звуков
        public void StopAllSounds()
        {
            foreach (var pool in _soundPools.Values)
            {
                pool.StopAll();
            }
        }

        // Возвращение экземпляра в пул после завершения воспроизведения
        private IEnumerator ReturnToPoolAfterPlay(EventInstance instance, FMOD.GUID soundGuid)
        {
            if (!instance.isValid())
            {
                yield break;
            }

            instance.getDescription(out var eventDescription);

            if (eventDescription.isValid())
            {
                eventDescription.getLength(out int length);
                yield return new WaitForSeconds(length / 1000f);
            }

            if (_soundPools.ContainsKey(soundGuid))
            {
                _soundPools[soundGuid].ReturnInstance(instance);
            }
        }

        public EventInstance PlaySoundWithInstance(EventReference soundPath, bool useInstance = false,
            Vector3? position = null)
        {
            if (soundPath.Guid.IsNull)
            {
                Debug.LogWarning("Sound reference is empty!");
                return default;
            }

            if (!useInstance)
            {
                // Воспроизведение одиночного звука
                if (position.HasValue)
                    RuntimeManager.PlayOneShot(soundPath, position.Value);
                else
                    RuntimeManager.PlayOneShot(soundPath);

                return default;
            }
            else
            {
                // Используем пул
                if (!_soundPools.ContainsKey(soundPath.Guid))
                {
                    // Debug.LogWarning($"Sound pool for {soundPath.Guid} not found. Initializing new pool.");
                    InitializeSoundPool(soundPath);
                }

                var instance = _soundPools[soundPath.Guid].GetInstance();

                // Устанавливаем 3D-атрибуты
                if (position.HasValue)
                    instance.set3DAttributes(RuntimeUtils.To3DAttributes(position.Value));
                else
                    instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

                instance.start();

                return instance; // Возвращаем экземпляр
            }
        }
    }
}