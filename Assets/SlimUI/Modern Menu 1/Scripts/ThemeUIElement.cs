using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlimUI.Modern_Menu_1.Scripts{
	[System.Serializable]
	public class ThemeUIElement : ThemeUI {
		[Header("Parameters")] private Color outline;
		private Image image;
		private GameObject message;
		public enum OutlineStyle {solidThin, solidThick, dottedThin, dottedThick};
		public bool hasImage = false;
		public bool isText = false;

		protected override void OnSkinUI(){
			base.OnSkinUI();

			if(hasImage){
				image = GetComponent<Image>();
				image.color = themeController.currentColor;
			}

			message = gameObject;

			if(isText){
				message.GetComponent<TextMeshPro>().color = themeController.textColor;
			}
		}
	}
}