﻿using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceShooter.States;

namespace SpaceShooter.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField]
        private GameStateType _nextState;

        public void StartGame()
        {
            //SceneManager.LoadScene(_levelName);
            GameStateController.PerformTransition(_nextState);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }

}
