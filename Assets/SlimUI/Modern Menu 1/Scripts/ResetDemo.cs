using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.Modern_Menu_1.Scripts{
	public class ResetDemo : MonoBehaviour {
		private void Update () {
			if(UnityEngine.Input.GetKeyDown("r")){
                SceneManager.LoadScene(0);
			}
		}
	}
}