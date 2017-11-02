using UnityEngine;
using UnityEngine.SceneManagement;
using SpaceShooter.States;
using TMPro;

namespace SpaceShooter.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _scoreText;

        [SerializeField]
        private TextMeshProUGUI _winText;

        // Called every time GameObject is activated
        // If GameObject is active when it is instantiated,
        // will be called right after Awake()
        private void OnEnable()
        {
            SetScore(GameManager.Instance.CurrentScore);
        }

        private void SetScore(int score)
        {
            if(_scoreText != null)
            {
                _scoreText.text = "Score : " + score;
            }

            if(_winText != null)
            {
                string text;
                if(GameManager.Instance.PlayerWins)
                {
                    text = "Player Wins!";
                }
                else
                {
                    text = "Player Loses!";
                }

                _winText.text = text;
            }
        }

        public void ToMainMenu()
        {
            GameStateController.PerformTransition(GameStateType.MainMenu);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }

}