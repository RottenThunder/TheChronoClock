using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IP
{
    public class GameMenu : MonoBehaviour
    {
        public GameObject gameMenu;
        public GameObject gameMenuButton;

        public void ShowGameMenu()
        {
            gameMenuButton.SetActive(false);
            gameMenu.SetActive(true);
        }

        public void HideGameMenu()
        {
            gameMenu.SetActive(false);
            gameMenuButton.SetActive(true);
        }
    }
}