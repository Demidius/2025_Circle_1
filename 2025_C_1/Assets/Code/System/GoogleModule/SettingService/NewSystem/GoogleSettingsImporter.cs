// using System.Collections.Generic;
//
// using UnityEngine;
//
// namespace CodeBase.System.Services.GoogleModule.SettingService.NewSystem
// {
//     public class GoogleSettingsImporter : MonoBehaviour
//     {
//         private const string InputJsonResourcePath = "Settings/GoogleSettingsUploader";
//
//         private Dictionary<string, SettingElement> _settingsMap;
//
//         private void Awake()
//         {
//             LoadSettings();
//         }
//
//         private void LoadSettings()
//         {
//             var textAsset = Resources.Load<TextAsset>(InputJsonResourcePath);
//             if (textAsset == null)
//             {
//                 Debug.LogError("[GoogleSettingsImporter] JSON-файл не найден в Resources.");
//                 return;
//             }
//
//             _settingsMap = JsonConvert.DeserializeObject<Dictionary<string, SettingElement>>(textAsset.text);
//
//             if (_settingsMap == null || _settingsMap.Count == 0)
//             {
//                 Debug.LogWarning("[GoogleSettingsImporter] Пустой или неверный JSON.");
//                 return;
//             }
//
//             // Debug.Log($"[GoogleSettingsImporter] Загружено параметров: {_settingsMap.Count}");
//         }
//
//         public SettingElement GetSetting(string key)
//         {
//             return _settingsMap != null && _settingsMap.TryGetValue(key, out var value) ? value : null;
//         }
//     }
//
//    
// }
