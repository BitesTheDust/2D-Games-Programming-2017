using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class GameManager : MonoBehaviour
    {
        #region Statics
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = Instantiate(Resources.Load<GameManager>("GameManager"));
                }

                return _instance;
            }
        }
        #endregion Statics

        [SerializeField, Tooltip("Amount of lives available for the player at the start.")]
        private int _startingLives;

        private int _currentLives;

        public int CurrentLives
        {
            get { return _currentLives; }
            set
            {
                _currentLives = value;
                if (_currentLives <= 0)
                {
                    _currentLives = 0;
                }
                if(LevelController.Current != null)
                {
                    LevelController.Current.LivesLost();
                }
            }
        }

        public int CurrentScore { get; private set; }

        public bool PlayerWins { get; set; }

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else if(_instance != this)
            {
                Destroy(gameObject);
                Debug.LogWarning("Destroying duplicate GameManager");
                return;
            }

            Init();
        }
        
        private void Init()
        {
            Debug.Log("Initializing GameManager");
            DontDestroyOnLoad(gameObject);
            Reset();
        }

        public void IncrementScore(int amount)
        {
            CurrentScore += amount;
 		}

        public void Reset()
        {
            _currentLives = _startingLives;
            CurrentScore = 0;
            PlayerWins = false;
        }
    }
}
