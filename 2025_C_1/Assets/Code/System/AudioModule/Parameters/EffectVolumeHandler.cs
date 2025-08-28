using CodeBase.System.Core.Consts;
using CodeBase.System.GameSystems.StateMachine.Core;
using CodeBase.System.GameSystems.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.System.GameSystems.AudioModule.Parameters
{
    public class EffectVolumeHandler : MonoBehaviour
    {
        [SerializeField] private Slider _volumeSliderMenu;
        [SerializeField] private Slider _volumeSliderSettings;
        [SerializeField] private Slider _volumeSliderPause;

        private Slider _activeSlider;
        private float _currentVolume;
        private bool _isSyncing = false;

        private GameStateMachine _gameStateMachine;
        private PlayerSettingsService _settings;

        [Inject]
        void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        private void Awake()
        {
            _settings = new PlayerSettingsService();
            _currentVolume = _settings.EffectVolume;

            InitSlider(_volumeSliderMenu);
            InitSlider(_volumeSliderSettings);
            InitSlider(_volumeSliderPause);

            _volumeSliderMenu.value = _currentVolume;
            _volumeSliderSettings.value = _currentVolume;
            _volumeSliderPause.value = _currentVolume;

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Const.Effectvolume, _currentVolume);
        }

        private void OnEnable()
        {
            _gameStateMachine.OnStateChanged += StateChanged;
        }

        private void OnDisable()
        {
            _gameStateMachine.OnStateChanged -= StateChanged;
        }

        private void InitSlider(Slider slider)
        {
            if (slider == null) return;
            slider.onValueChanged.AddListener(volume => OnSliderChanged(slider, volume));
        }

        private void OnSliderChanged(Slider changedSlider, float volume)
        {
            if (_isSyncing) return;

            _isSyncing = true;

            _currentVolume = volume;
            _settings.EffectVolume = volume;
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName(Const.Effectvolume, volume);

            if (changedSlider == _volumeSliderMenu)
                _volumeSliderSettings.value = volume;
            else if (changedSlider == _volumeSliderSettings)
                _volumeSliderMenu.value = volume;

            _isSyncing = false;
        }

        private void StateChanged(GameState state)
        {
            if (state is MenuState)
                _activeSlider = _volumeSliderMenu;
            else if (state is PauseState)
                _activeSlider = _volumeSliderPause;

            if (_activeSlider != null)
                _activeSlider.value = _currentVolume;
        }
    }
}
