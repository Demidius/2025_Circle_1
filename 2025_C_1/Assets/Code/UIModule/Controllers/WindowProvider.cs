using System;
using System.Collections.Generic;
using CodeBase._2UIModuleF.Windows;
using UnityEngine;
namespace CodeBase._2UIModuleF.UIControllers
{
    public class WindowProvider : AudioSours
    {
        private UIModuleContainer _parent;
        private Dictionary<Type, GameObject> _windows;
        private BaseWindowUI _currentWindow;
        public WindowProvider(UIModuleContainer parent)
        {
            _parent = parent;
            
            _windows = new Dictionary<Type, GameObject>();
            RegisterWindows();
        }
        private void RegisterWindows()
        {
            BaseWindowUI[] windowUis = _parent.GetComponentsInChildren<BaseWindowUI>(true);

            foreach (var window in windowUis)
            {
                Type windowType = window.GetType();

                if (_windows.ContainsKey(windowType))
                {
                    Debug.LogWarning($"Окно типа {windowType.Name} уже зарегистрировано.");
                    continue;
                }

                _windows.Add(windowType, window.gameObject);
            }
        }
        
        public void ShowWindow<T>() where T : BaseWindowUI
        {
            foreach (var kvp in _windows)
            {
                bool isTarget = kvp.Key == typeof(T);
                kvp.Value.SetActive(isTarget);

                if (isTarget)
                {
                    _currentWindow = kvp.Value.GetComponent<BaseWindowUI>();
                }
            }
        }
        
        public void ResetAllWindows()
        {
            foreach (var window in _windows.Values)
                window.SetActive(false);

            _currentWindow = null;
        }
     
        public BaseWindowUI CurrentWindow => _currentWindow;
        
    }

}
