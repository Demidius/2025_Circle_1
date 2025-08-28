using System;
using CodeBase._2UIModuleF.UIControllers;
using UnityEngine;
namespace CodeBase._2UIModuleF.Buttons
{
    public class UIButtonsConnector : MonoBehaviour
    {
        
        [SerializeField] private GameObject _buttonContainer;
        
        private void Awake()
        {
            FindButtons();
           
        }
        private void Start()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            foreach (BaseButtonEvent button in _buttons)
            {
                button.Clicked -= OnClick;
            }
        }

        private UIModuleContainer _parent;
        private BaseButtonEvent[] _buttons;
        public event Action<BaseButtonEvent> Clicked;

        private void FindButtons()
        {
            _buttons = _buttonContainer.GetComponentsInChildren<BaseButtonEvent>(true) ?? Array.Empty<BaseButtonEvent>();
        }

        private void Subscribe()
        {
            foreach (BaseButtonEvent button in _buttons)
            {
                button.Clicked += OnClick;
            }
        }

        private void OnClick(BaseButtonEvent obj)
        {
            Clicked?.Invoke(obj);
        }
       
    }
}
