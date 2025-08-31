using System;
using UnityEngine;

namespace Code.GameEntyties.Player // если можно, поправь на GameEntities
{

    public class TanksEngine : MonoBehaviour, ITanksEngine
    {
    public event Action<bool> ChangeEngineState;
    public event Action<float> ChangeRpm;

    [Header("Двигатель")]
    [SerializeField] private bool _engineStarted;
    [SerializeField] private float _maxEngineRpm = 3000f;
    [SerializeField] private float _minEngineRpm = 800f; // холостой ход

    [Tooltip("Скорость набора оборотов (ед/с)")]
    [SerializeField] private float _accelUp = 400f;
    [Tooltip("Скорость снижения оборотов (ед/с)")]
    [SerializeField] private float _accelDown = 200f;

    private float _engineRpm; // текущие обороты
    private float _prevRpm;
    private int _throttle; 
    public float EngineRpm  => _engineRpm; 

    private void Awake()
    {
        if (_engineStarted)
            _engineRpm = Mathf.Max(_minEngineRpm, 0f);
        else
            _engineRpm = 0f;
    }

    private void Update()
    {
        // Переключение двигателя
        if (Input.GetKeyDown(KeyCode.I))
        {
            _engineStarted = !_engineStarted;
            ChangeEngineState?.Invoke(_engineStarted);

            // мгновенный переход оборотов при выключении
            if (!_engineStarted && _engineRpm != 0f)
            {
                _prevRpm = _engineRpm;
                _engineRpm = 0f;
                ChangeRpm?.Invoke(_engineRpm);
            }
            // при включении — минимум (холостой)
            if (_engineStarted && _engineRpm < _minEngineRpm)
            {
                _prevRpm = _engineRpm;
                _engineRpm = _minEngineRpm;
                ChangeRpm?.Invoke(_engineRpm);
            }
        }

        // Кэш ввода (лучше в Update)
        _throttle =
            Input.GetKey(KeyCode.W) ? +1 : Input.GetKey(KeyCode.S) ? -1 : 0 ;

        // Debug.Log(_engineRpm);
    }

    private void FixedUpdate()
    {
        _prevRpm = _engineRpm;

        if (!_engineStarted)
        {
            _engineRpm = 0f;
        }
        else
        {
            float dt = Time.fixedDeltaTime;

            if (_throttle > 0)
            {
                // Разгон к максимальным
                _engineRpm = Mathf.MoveTowards(_engineRpm, _maxEngineRpm, _accelUp * dt);
            }
            else if (_throttle < 0)
            {
                _engineRpm = Mathf.MoveTowards(_engineRpm, _minEngineRpm, _accelDown * dt);
            }
            else
            {
                
            }

            // Границы
            _engineRpm = Mathf.Clamp(_engineRpm, _minEngineRpm, _maxEngineRpm);
            
            // Debug.Log(_engineRpm);
        }

        // Событие изменения оборотов
        if (!Mathf.Approximately(_prevRpm, _engineRpm))
            ChangeRpm?.Invoke(_engineRpm);
    }
    }

}
