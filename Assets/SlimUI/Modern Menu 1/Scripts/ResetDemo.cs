using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{
	public class ResetDemo : MonoBehaviour {

		void Update () {
			if(UnityEngine.Input.GetKeyDown("r")){
                SceneManager.LoadScene(0);
			}
		}
	}
}