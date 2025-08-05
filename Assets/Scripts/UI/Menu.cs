using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Others
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            startButton.onClick.AddListener(GameStart);
            exitButton.onClick.AddListener(GameExit);
        }

        private static void GameStart()
        {
            SceneManager.LoadScene(1);
        }

        private static void GameExit()
        {
            Application.Quit();
        }
    }
}
