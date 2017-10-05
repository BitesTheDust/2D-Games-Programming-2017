﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private string _levelName = "level_01";


        public void StartGame()
        {
            SceneManager.LoadScene(_levelName);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }

}
