// using System;
// using System.Collections.Generic;
// using Unity.Plastic.Newtonsoft.Json;
// using UnityEngine;
//
// namespace CodeBase.System.GameSystems.Localization
// {
//     public enum LocalisationName
//     {
//         LocalizationText_RU,
//         LocalizationText_EN
//     }
//
//     public class LocalisationManager : MonoBehaviour
//     {
//         public event Action<LocalisationName> OnLanguageChanged;
//         private Dictionary<string, string> _localisationsMap = new();
//
//         private void Awake()
//         {
//             LoadLocalization(LocalisationName.LocalizationText_RU);
//         }
//
//         public string GetText(string id)
//         {
//             return _localisationsMap.TryGetValue(id, out var txt)
//                 ? txt
//                 : $"[Missing: {id}]";
//         }
//
//         public void SetRussianLanguage()  => LoadLocalization(LocalisationName.LocalizationText_RU);
//         public void SetEnglishLanguage()  => LoadLocalization(LocalisationName.LocalizationText_EN);
//
//         private void Update()
//         {
//             if (UnityEngine.Input.GetKeyDown(KeyCode.I))
//                 SetRussianLanguage();
//             else if (UnityEngine.Input.GetKeyDown(KeyCode.O))
//                 SetEnglishLanguage();
//         }
//
//         private void LoadLocalization(LocalisationName resourceName)
//         {
//             var txtAsset = Resources.Load<TextAsset>("Localization/" + resourceName);
//             if (txtAsset == null)
//             {
//                 Debug.LogError($"[Localization] Не найден: Resources/Localization/{resourceName}.json");
//                 return;
//             }
//
//             _localisationsMap = ParseJsonToDictionary(txtAsset.text);
//             if (_localisationsMap.Count == 0)
//             {
//                 Debug.LogError($"[Localization] Не удалось распарсить словарь: {resourceName}");
//                 return;
//             }
//
//             OnLanguageChanged?.Invoke(resourceName);
//         }
//
//         private Dictionary<string, string> ParseJsonToDictionary(string json)
//         {
//             try
//             {
//                 return JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
//                        ?? new Dictionary<string, string>();
//             }
//             catch (JsonException ex)
//             {
//                 Debug.LogError($"[Localization] Ошибка парсинга JSON: {ex.Message}");
//                 return new Dictionary<string, string>();
//             }
//         }
//     }
// }
