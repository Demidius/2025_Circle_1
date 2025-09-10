using System;
using UnityEngine;

namespace Code.GameEntyties.Player // если можно, поправь на GameEntities
{

    public class TanksEngine : MonoBehaviour, ITanksEngine
    {
        public event Action<bool> ChangeEngineState;

        [Header("Двигатель")]
        [SerializeField] private bool _engineStarted;
        public bool IsOn
        {
            get
            {
                return _engineStarted;
            }
            set
            {
                _engineStarted = value;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                _engineStarted = !_engineStarted;
                ChangeEngineState?.Invoke(_engineStarted);
            }
        }
    }
}
