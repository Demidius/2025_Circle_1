using UnityEngine;
using UnityEngine.UI;
namespace CodeBase._2UIModuleF.Buttons
{
	public class LinkButton : MonoBehaviour
	{
		public string url = ""; 

		private void Start()
		{
			Button button = GetComponent<Button>();
			if (button != null)
			{
				button.onClick.AddListener(OpenLink);
			}

			if (url == "")
			{
				Debug.LogWarning("Link button URL is empty");
			}
		}

		public void OpenLink()
		{
			Application.OpenURL(url);
		}
	}
}
