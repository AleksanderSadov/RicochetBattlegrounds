using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.Ricochet.Game
{
    public class TriggerLoadScene : MonoBehaviour
    {
        public string sceneName;

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.CompareTag("Player"))
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
