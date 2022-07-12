using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Ricochet.Game
{
    public class GameManager : MonoBehaviour
    {
        public GameObject endSection;
        public bool isGameOver = false;

        private void Start()
        {
            EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.AddListener<AllEnemiesKilled>(OnAllEnemiesKilled);
        }

        private void Update()
        {
            if (isGameOver && Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void OnPlayerDeath(PlayerDeathEvent evt)
        {
            isGameOver = true;
            EventManager.Broadcast(new GameOverEvent());
        }

        private void OnAllEnemiesKilled(AllEnemiesKilled evt)
        {
            endSection.SetActive(true);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
            EventManager.RemoveListener<AllEnemiesKilled>(OnAllEnemiesKilled);
        }
    }
}
