// using TMPro;
// using UnityEngine;
// using Zenject;
// namespace CodeBase.System.GameSystems.Localization
// {
//     public class LocalisationUISwitcher : MonoBehaviour
//     {
//         [Inject] private LocalisationManager _localisationManager;
//         
//         [SerializeField] private TMP_Dropdown  _dropdown;
//
//         private void Start()
//         {
//             _dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
//         }
//
//         private void OnDisable()
//         {
//             _dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
//         }
//
//
//
//
//         private void OnDropdownValueChanged(int index)
//         {
//             switch (index)
//             {
//                 case 0:
//                     _localisationManager.SetRussianLanguage();
//                     break;
//                 case 1:
//                     _localisationManager.SetEnglishLanguage();
//                     break;
//                 default:
//                     _localisationManager.SetEnglishLanguage();
//                     Debug.LogError("Error setting localisation language");
//                     break;
//                 
//             }
//         }
//     }
// }
