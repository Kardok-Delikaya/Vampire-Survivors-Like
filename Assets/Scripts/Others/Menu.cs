using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        public void GameStart()
        {
            SceneManager.LoadScene("Game");
        }

        public void GameExit()
        {
            Application.Quit();
        }
    }
}
