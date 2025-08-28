using System;
using UnityEngine;
using UnityEngine.UI;
namespace CodeBase._2UIModuleF.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseButtonEvent : MonoBehaviour
    {
        public event Action<BaseButtonEvent> Clicked;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Clicked?.Invoke(this);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}
