using CodeBase.System.Core.Consts;
using CodeBase.System.GameSystems.StateMachine.Core;
using CodeBase.System.GameSystems.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;

namespace CodeBase.System.GameSystems.AudioModule.Parameters
{
    public class EffectVolumeHandler : MonoBehaviour
    {
        [Header("Любое количество слайдеров громкости эффектов")]
        [SerializeField] private List<Slider> _sliders = new(); // можно 0..∞

        private float _currentVolume;
        private bool _isSyncing;

        private GameStateMachine _gameStateMachine;
        private PlayerSettingsService _settings;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Awake()
        {
            _settings = new PlayerSettingsService();
            _currentVolume = _settings.EffectVolume;

            // подписываем все слайдеры
            for (int i = 0; i < _sliders.Count; i++)
            {
                var slider = _sliders[i];
                if (slider == null) continue;
                slider.onValueChanged.AddListener(v => OnSliderChanged(slider, v));
                slider.value = _currentVolume; // стартовая синхронизация
            }

            ApplyVolume(_currentVolume);
        }

        private void OnEnable()
        {
            if (_gameStateMachine != null)
                _gameStateMachine.OnStateChanged += OnStateChanged;
        }

        private void OnDisable()
        {
            if (_gameStateMachine != null)
                _gameStateMachine.OnStateChanged -= OnStateChanged;

            // важно снять подписки, если объект могут уничтожать/создавать
            for (int i = 0; i < _sliders.Count; i++)
            {
                var slider = _sliders[i];
                if (slider == null) continue;
                slider.onValueChanged.RemoveAllListeners();
            }
        }

        private void OnSliderChanged(Slider source, float volume)
        {
            if (_isSyncing) return;

            _isSyncing = true;

            _currentVolume = volume;
            _settings.EffectVolume = volume;
            ApplyVolume(volume);

            // обновляем все остальные слайдеры
            for (int i = 0; i < _sliders.Count; i++)
            {
                var s = _sliders[i];
                if (s == null || s == source) continue;
                s.value = volume;
            }

            _isSyncing = false;
        }

        private void ApplyVolume(float volume)
        {
            // тут же прокидываем в FMOD параметр
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Const.Effectvolume, volume);
        }

        private void OnStateChanged(GameState state)
        {
            // При смене стейта можно:
            // 1) просто синкнуть все слайдеры с текущим значением:
            _isSyncing = true;
            for (int i = 0; i < _sliders.Count; i++)
            {
                var s = _sliders[i];
                if (s == null) continue;
                s.value = _currentVolume;
            }
            _isSyncing = false;

            // Если нужно на конкретных экранах обновлять только видимые — 
            // можно фильтровать по s.gameObject.activeInHierarchy.
        }
    }
}
