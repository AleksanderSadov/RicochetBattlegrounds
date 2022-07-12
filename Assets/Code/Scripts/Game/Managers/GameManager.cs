using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Ricochet.Game
{
    public class GameManager : MonoBehaviour
    {
        public GameObject restartMessage, endSection;
        static GameManager myslf;
        public bool gameOver = false;
        int enemyCount;
        void Awake()
        {
            myslf = this;

        }
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (gameOver && Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }

        public static void RegisterPlayerDeath()
        {
            myslf.restartMessage.SetActive(true);
            myslf.restartMessage.transform.localScale = Vector3.one * 2.0f;
            iTween.Stop(myslf.restartMessage.gameObject);
            iTween.ScaleTo(myslf.restartMessage, iTween.Hash("scale", Vector3.one, "time", 0.5f, "delay", 0.1f, "easetype", iTween.EaseType.spring));
            myslf.gameOver = true;
        }
        public static void AddToEnemyCount()
        {
            myslf.enemyCount++;
        }
        public static void RemoveEnemy()
        {
            myslf.enemyCount--;
            if (myslf.enemyCount <= 0)
            {
                myslf.endSection.SetActive(true);
            }

        }
    }
}
