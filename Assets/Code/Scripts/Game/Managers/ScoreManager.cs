using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class ScoreManager : MonoBehaviour
    {
        public int playerScore = 0;
        public int enemyScore = 0;

        public static ScoreManager Instance;

        private GameManager gameManager;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.AddListener<AllEnemiesKilled>(OnAllEnemiesKilled);
        }

        public void AddPlayerScore()
        {
            if (gameManager.isGameOver)
            {
                return;
            }

            playerScore++;
        }

        public void AddEnemyScore()
        {
            if (gameManager.isGameOver)
            {
                return;
            }

            enemyScore++;
        }

        private void OnPlayerDeath(PlayerDeathEvent evt)
        {
            AddEnemyScore();
        }

        private void OnAllEnemiesKilled(AllEnemiesKilled evt)
        {
            AddPlayerScore();
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.RemoveListener<AllEnemiesKilled>(OnAllEnemiesKilled);
        }
    }
}
