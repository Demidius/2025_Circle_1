// #if UNITY_EDITOR
// #nullable enable
// using System.Collections.Generic;
// using System.Globalization;
// using System.IO;
// using System.Net;
// using System.Text;
// using UnityEditor;
// using UnityEngine;
//
// namespace CodeBase.System.Services.GoogleModule.SettingService.NewSystem
// {
//     public static class GoogleSettingsUploader
//     {
//         private const string OutputFolder = "Assets/Resources/Settings";
//         private static readonly string SettingSheetUrl =
//             "https://docs.google.com/spreadsheets/d/e/2PACX-1vR33MkWX1IxJAOHHjajmuJtN2fNtqEZrYTsc6NKH6EYNQMIOogWLa-pFrJxYcpErafHOHuZC3Gfz7N_/pubhtml?gid=0&single=true";
//
//         [MenuItem("Tools/Settings/Update JSON From Google Sheet")]
//         public static void UpdateJson()
//         {
//             Directory.CreateDirectory(OutputFolder);
//
//             string? tsv = null;
//             try
//             {
//                 using var client = new WebClient();
//                 tsv = client.DownloadString(SettingSheetUrl);
//             }
//             catch (WebException ex)
//             {
//                 Debug.LogError($"[GoogleSettingsUploader] Не смог скачать: {ex.Message}");
//                 return;
//             }
//
//             if (string.IsNullOrWhiteSpace(tsv))
//             {
//                 Debug.LogWarning("[GoogleSettingsUploader] TSV пуст.");
//                 return;
//             }
//
//             var dict = ParseTsv(tsv);
//             if (dict.Count == 0)
//             {
//                 Debug.LogWarning("[GoogleSettingsUploader] Таблица пуста или не разобрана.");
//                 return;
//             }
//
//             var json = DictionaryToJson(dict);
//             var path = Path.Combine(OutputFolder, "GoogleSettingsUploader.json");
//             File.WriteAllText(path, json, Encoding.UTF8);
//
//             Debug.Log($"[GoogleSettingsUploader] JSON сохранён по пути: {path}");
//         }
//
//         private static Dictionary<string, SettingElement> ParseTsv(string raw)
//         {
//             var dict = new Dictionary<string, SettingElement>();
//             using var sr = new StringReader(raw);
//             _ = sr.ReadLine();                       // пропускаем заголовок
//
//             string? line;
//             while ((line = sr.ReadLine()) != null)
//             {
//                 if (string.IsNullOrWhiteSpace(line)) continue;
//
//                 var parts = line.Split('\t');
//
//                 // Первые две колонки (ID и Name) обязательны
//                 if (parts.Length < 2) continue;
//
//                 var id   = parts[0].Trim();
//                 var name = parts[1].Trim();
//                 if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name)) continue;
//
//                 // --- Далее — опциональные значения ---
//                 // FloatValue
//                 float floatVal = 0f;
//                 if (parts.Length > 2 &&              // колонка существует
//                     !string.IsNullOrWhiteSpace(parts[2]) &&
//                     float.TryParse(parts[2],
//                         NumberStyles.Float,
//                         CultureInfo.InvariantCulture,
//                         out var parsedFloat))
//                 {
//                     floatVal = parsedFloat;
//                 }
//
//                 // IntValue
//                 int intVal = 0;
//                 if (parts.Length > 3 &&
//                     !string.IsNullOrWhiteSpace(parts[3]) &&
//                     int.TryParse(parts[3],
//                         NumberStyles.Integer,
//                         CultureInfo.InvariantCulture,
//                         out var parsedInt))
//                 {
//                     intVal = parsedInt;
//                 }
//
//                 // StringValue
//                 var stringVal = (parts.Length > 4) ? parts[4].Trim() : string.Empty;
//
//                 dict[id] = new SettingElement
//                 {
//                     Id          = id,
//                     Name        = name,
//                     FloatValue  = floatVal,
//                     IntValue    = intVal,
//                     StringValue = stringVal
//                 };
//             }
//
//             return dict;
//         }
//
//
//         private static string DictionaryToJson(Dictionary<string, SettingElement> dict)
//         {
//             var sb = new StringBuilder();
//             sb.Append("{\n");
//             int i = 0;
//             foreach (var (key, element) in dict)
//             {
//                 sb.Append("  \"").Append(Escape(key)).Append("\": {\n");
//                 sb.Append("    \"Name\": \"").Append(Escape(element.Name)).Append("\",\n");
//                 sb.Append("    \"FloatValue\": ").Append(element.FloatValue.ToString(CultureInfo.InvariantCulture)).Append(",\n");
//                 sb.Append("    \"IntValue\": ").Append(element.IntValue).Append(",\n");
//                 sb.Append("    \"StringValue\": \"").Append(Escape(element.StringValue)).Append("\"\n");
//                 sb.Append("  }");
//                 if (i < dict.Count - 1) sb.Append(',');
//                 sb.Append('\n');
//                 i++;
//             }
//             sb.Append('}');
//             return sb.ToString();
//         }
//
//         private static string Escape(string s) => s.Replace("\\", "\\\\").Replace("\"", "\\\"");
//     }
// }
// #endif