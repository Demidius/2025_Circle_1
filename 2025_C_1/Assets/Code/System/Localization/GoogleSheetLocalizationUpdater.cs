// #if UNITY_EDITOR
// #nullable enable
// using System.Collections.Generic;
// using System.IO;
// using System.Net;
// using System.Text;
//
// using UnityEditor;
// using UnityEngine;
// namespace CodeBase.System.GameSystems.Localization
// {
//     /// <summary>
//     /// Забирает опубликованные TSV-файлы из Google Sheets (по одному на каждую вкладку),
//     /// конвертирует их в JSON вида { "ID": "Text" } и кладёт в Assets/Resources/Localization.
//     /// Для вызова нажми «Tools ▸ Localization ▸ Update JSON From Google Sheet».
//     /// </summary>
//     public static class GoogleSheetLocalizationUpdater
//     {
//         private const string OutputFolder = "Assets/Resources/Localization";
//
//         private static readonly Dictionary<string, string> SheetUrls = new()
//         {
//             {"EN", "https://docs.google.com/spreadsheets/d/e/2PACX-1vQSteQNHSJfNg35JJsxixwxTTQisvRvRhF5iJUhkaop5Hql1BM6u4cQX9d87iU0zfQqORrZU_Mo0QaD/pub?gid=0&single=true&output=tsv"},
//             {"RU", "https://docs.google.com/spreadsheets/d/e/2PACX-1vQSteQNHSJfNg35JJsxixwxTTQisvRvRhF5iJUhkaop5Hql1BM6u4cQX9d87iU0zfQqORrZU_Mo0QaD/pub?gid=1202285216&single=true&output=tsv"}
//         };
//
//         [MenuItem("Tools/Localization/Update JSON From Google Sheet")]
//         public static void UpdateLocalizationJson()
//         {
//             Directory.CreateDirectory(OutputFolder);
//
//             foreach (var kv in SheetUrls)
//             {
//                 var lang = kv.Key;
//                 var url  = kv.Value;
//                 string tsv;
//
//                 // --- Скачиваем TSV ---
//                 try
//                 {
//                     using var client = new WebClient();
//                     tsv = client.DownloadString(url);
//                 }
//                 catch (WebException ex)
//                 {
//                     Debug.LogError($"[Localization] Не смог скачать {lang}: {ex.Message}");
//                     continue;
//                 }
//
//                 // --- Парсим TSV -> Dictionary ---
//                 var dict = ParseTsv(tsv);
//                 if (dict.Count == 0)
//                 {
//                     Debug.LogWarning($"[Localization] Таблица {lang} пуста или не разобрана.");
//                     continue;
//                 }
//
//                 // --- Превращаем в JSON через Json.NET ---
//                 var json  = JsonConvert.SerializeObject(dict, Formatting.Indented);
//                 var path  = Path.Combine(OutputFolder, $"LocalizationText_{lang}.json");
//
//                 // --- Сохраняем, если что-то поменялось ---
//                 if (File.Exists(path) && File.ReadAllText(path, Encoding.UTF8) == json)
//                 {
//                     Debug.Log($"[Localization] {lang} уже актуален. Пропускаю.");
//                     continue;
//                 }
//
//                 File.WriteAllText(path, json, Encoding.UTF8);
//                 Debug.Log($"[Localization] Записал/обновил {path}");
//             }
//
//             AssetDatabase.Refresh();
//         }
//
//         private static Dictionary<string, string> ParseTsv(string raw)
//         {
//             var dict = new Dictionary<string, string>();
//             using var sr = new StringReader(raw);
//             _ = sr.ReadLine(); // шапку пропустили
//             string? line;
//             while ((line = sr.ReadLine()) != null)
//             {
//                 var parts = line.Split('\t');
//                 if (parts.Length < 2) continue;
//                 var id   = parts[0].Trim();
//                 var text = parts[1].Trim();
//                 if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(text)) continue;
//                 dict[id] = text;
//             }
//             return dict;
//         }
//     }
// }
// #endif
