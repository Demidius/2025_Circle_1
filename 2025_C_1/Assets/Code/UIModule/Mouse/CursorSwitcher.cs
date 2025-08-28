using CodeBase.System.Core.Consts;
using CodeBase.System.GameSystems.Input;
using CodeBase.System.GameSystems.StateMachine.Core;
using CodeBase.System.GameSystems.StateMachine.States;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Cursor = UnityEngine.Cursor;



namespace CodeBase._2UIModuleF.Mouse
{

    public enum MouseType
    {
        MenuMode,
        GameMode
    }


    public class UICursor : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private GameObject _cursorGO;
        [SerializeField] private Vector2 _hotspotOffset;

        [SerializeField] private GameObject _menuTypeSprite;
        [SerializeField] private GameObject _gameTypeSprite;
        [SerializeField] private GameObject _basicalBox;


        [SerializeField] private Slider _cursorScaleSlider;



        [Inject] IInputCase _inputCase;
        [Inject] GameStateMachine _gameStateMachine;

        private PlayerSettingsService _settings;
        private float _currentScale;
        private Vector2 _mousePos;
        private RectTransform _cursorRect;

        private void Awake()
        {
            _settings = new PlayerSettingsService();

            // Установка начального значения масштаба
            _currentScale = _settings.CursorSacale > 0 ? _settings.CursorSacale : 0.5f;

            // Установка значения слайдера до инициализации
            if (_cursorScaleSlider != null)
                _cursorScaleSlider.value = _currentScale;

            InitSlider(_cursorScaleSlider);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;

            _cursorRect = _cursorGO.GetComponent<RectTransform>();
            if (_cursorRect == null)
                Debug.LogError("Cursor GameObject must have a RectTransform component!");

            _inputCase.OnSystemMousePoint += MosePos;
            _gameStateMachine.OnStateChanged += TypeChange;

            ApplyCursorScale(_currentScale);
        }

        private void OnDestroy()
        {
            _inputCase.OnSystemMousePoint -= MosePos;
            _gameStateMachine.OnStateChanged -= TypeChange;
        }

        private void MosePos(Vector2 pos)
        {
            _mousePos = pos;
        }

        private void Update()
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvas.GetComponent<RectTransform>(),
                _mousePos,
                _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera,
                out localPos);

            InitSlider(_cursorScaleSlider);

            _cursorRect.anchoredPosition = localPos + _hotspotOffset;
        }
        private void ApplyCursorScale(float volume)
        {
            _basicalBox.transform.localScale = new Vector3(volume + 0.3f, volume + 0.3f, volume + 0.3f);
        }

        private void InitSlider(Slider slider)
        {
            if (slider == null) return;

            // Отписываемся, чтобы не дублировать при каждом кадре
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(OnSliderChanged);
        }
        private void OnSliderChanged(float volume)
        {
            _settings.CursorSacale = volume;
            _currentScale = volume;
            ApplyCursorScale(volume);
        }

        private void TypeChange(GameState state)
        {
            switch (state)
            {
                case GameplayState:
                    _gameTypeSprite.SetActive(true);
                    _menuTypeSprite.SetActive(false);
                    break;
                default:
                    _gameTypeSprite.SetActive(false);
                    _menuTypeSprite.SetActive(true);
                    break;
            }
        }

        public void MouseChange(MouseType mouseType)
        {
            switch (mouseType)
            {
                case MouseType.MenuMode:
                    _gameTypeSprite.SetActive(false);
                    _menuTypeSprite.SetActive(true);
                    break;
                case MouseType.GameMode:
                   
                    _gameTypeSprite.SetActive(true);
                    _menuTypeSprite.SetActive(false);
                    break;
            }
        }


    }
}
