using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class ScoreManager : MonoBehaviour
    {
        public int playerScore = 0;
        public int enemyScore = 0;

        public static ScoreManager Instance;

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
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.AddListener<AllEnemiesKilled>(OnAllEnemiesKilled);
        }

        public void AddPlayerScore()
        {
            playerScore++;
        }

        public void AddEnemyScore()
        {
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
