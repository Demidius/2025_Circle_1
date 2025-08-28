// using TMPro;
// using UnityEngine;
// using Zenject;
// namespace CodeBase.System.GameSystems.Localization
// {
//     public class TextLoc : MonoBehaviour
//     {
//         [Inject] private LocalisationManager _localisationManager;
//
//         [SerializeField] private string _textID;
//         
//         private TextMeshProUGUI _textMesh;
//
//         private void OnEnable()
//         {
//             UpdateText();
//         }
//
//         private void Awake()
//         {
//             if (_textID == null)
//             {
//                 Debug.LogError($"[TextLoc] Text ID is null");
//             }
//             
//             _textMesh = GetComponent<TextMeshProUGUI>();
//             _localisationManager.OnLanguageChanged += UpdateText;
//
//         }
//
//         private void UpdateText(LocalisationName language)
//         {
//             _textMesh.text = _localisationManager.GetText(_textID);
//         }
//         private void UpdateText()
//         {
//             _textMesh.text = _localisationManager.GetText(_textID);
//         }
//
//
//         private void OnDestroy()
//         {
//             _localisationManager.OnLanguageChanged -= UpdateText;
//         }
//     }
// }
