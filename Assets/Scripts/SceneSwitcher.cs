using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IP
{
    public class SceneSwitcher : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene(2);
        }

        public void SeeBackStory()
        {
            SceneManager.LoadScene(1);
        }

        public void ToTitleScreen()
        {
            SceneManager.LoadScene(0);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}