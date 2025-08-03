using UnityEngine;

namespace SlimUI.Modern_Menu_1.Scripts
{
	[ExecuteInEditMode()]
	[System.Serializable]
	public class ThemeUI : MonoBehaviour
	{
		public ThemeEditor themeController;

		protected virtual void OnSkinUI()
		{

		}

		public virtual void Awake()
		{
			OnSkinUI();
		}

		public virtual void Update()
		{
			OnSkinUI();
		}
	}
}
