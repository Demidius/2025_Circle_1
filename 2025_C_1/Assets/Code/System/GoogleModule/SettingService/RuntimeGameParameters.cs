// using CodeBase.System.Services.GoogleModule.SettingService.NewSystem;
// using UnityEngine;
// using Zenject;
//
// namespace CodeBase.System.Services.GoogleModule.SettingService
// {
//     /// <summary>
//     /// Заполняет публичные поля данными из GoogleSettingsImporter
//     /// по жёстко заданным ключам.
//     /// </summary>
//     public class RuntimeGameParameters : IInitializable
//     {
//         [Inject] private readonly GoogleSettingsImporter _importer;
//
//         // ---------- ПАРАМЕТРЫ ----------
//
//         // Floats
//         // public float EnemyType1MoveSpeed { get; private set; }
//       
//         // Ints
//         // public int HeroBodyRotationSpeed { get; private set; }
//       
//         // IInitializable
//         public void Initialize() => LoadParameters();
//
//         private void LoadParameters()
//         {
//             if (_importer == null)
//             {
//                 Debug.LogError("RuntimeGameParameters: GoogleSettingsImporter не внедрён!");
//                 return;
//             }
//
//             // Floats
//             // EnemyType1MoveSpeed = GetFloat("A001");
//         
//         }
//
//         // Эти методы _внутри_ класса, чтобы LoadParameters видел их без ошибок
//         private float GetFloat(string key)
//         {
//             var el = _importer.GetSetting(key);
//             if (el == null)
//             {
//                 Debug.LogWarning($"[RuntimeGameParameters] ключ '{key}' не найден");
//                 return default;
//             }
//             return el.FloatValue;
//         }
//
//         private int GetInt(string key)
//         {
//             var el = _importer.GetSetting(key);
//             if (el == null)
//             {
//                 Debug.LogWarning($"[RuntimeGameParameters] ключ '{key}' не найден");
//                 return default;
//             }
//             return el.IntValue;
//         }
//     }
// }
