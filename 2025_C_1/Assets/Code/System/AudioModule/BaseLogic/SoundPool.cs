using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
namespace CodeBase.System.GameSystems.AudioModule.BaseLogic
{
    public class SoundPool
    {
        private Queue<EventInstance> pool = new Queue<EventInstance>();
        private List<EventInstance> activeInstances = new List<EventInstance>();
        private EventReference eventReference;

        public SoundPool(EventReference eventRef, int initialCount = 5)
        {
            if (eventRef.Guid.IsNull)
            {
               Debug.Log("Sound Reference is empty");
               return;
            }

            this.eventReference = eventRef;

            for (int i = 0; i < initialCount; i++)
            {
                var instance = RuntimeManager.CreateInstance(eventRef);
                Set3DAttributes(instance, Vector3.zero); // Устанавливаем дефолтные 3D атрибуты
                pool.Enqueue(instance);
            }
        }

        public EventInstance GetInstance(Vector3 position = default)
        {
            EventInstance instance;

            if (pool.Count > 0)
            {
                instance = pool.Dequeue();
            }
            else
            {
                instance = RuntimeManager.CreateInstance(eventReference);
            }

            Set3DAttributes(instance, position); // Устанавливаем 3D атрибуты при выдаче
            activeInstances.Add(instance); // Добавляем в список активных
            return instance;
        }

        public void ReturnInstance(EventInstance instance)
        {
            if (instance.isValid())
            {
                instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                Set3DAttributes(instance, Vector3.zero); // Сбрасываем 3D атрибуты
                activeInstances.Remove(instance); // Убираем из списка активных
                pool.Enqueue(instance); // Возвращаем в пул
            }
        }

        public void StopAll()
        {
            // Останавливаем все активные экземпляры
            foreach (var instance in activeInstances)
            {
                if (instance.isValid())
                {
                    instance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                }
            }

            // Возвращаем все активные экземпляры в пул
            List<EventInstance> instancesToReturn = new List<EventInstance>(activeInstances);
            foreach (var instance in instancesToReturn)
            {
                ReturnInstance(instance);
            }
        }

        public void ClearPool()
        {
            // Очищаем активные звуки
            StopAll();

            // Очищаем пул
            while (pool.Count > 0)
            {
                var instance = pool.Dequeue();
                if (instance.isValid())
                {
                    instance.release();
                }
            }
        }

        private void Set3DAttributes(EventInstance instance, Vector3 position)
        {
            if (instance.isValid())
            {
                var attributes = RuntimeUtils.To3DAttributes(position);
                instance.set3DAttributes(attributes);
            }
        }
    }
}
