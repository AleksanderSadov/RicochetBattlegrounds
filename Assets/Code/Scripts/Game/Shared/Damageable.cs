using UnityEngine;
using UnityEngine.Events;

namespace Unity.Ricochet.Game
{
    public class Damageable : MonoBehaviour
    {
        public UnityAction OnDie;

        public AudioClip deathAudioClip;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        public void Kill()
        {
            if (gameManager.isGameOver)
            {
                return;
            }

            OnDie?.Invoke();

            if (deathAudioClip != null)
            {
                AudioSource.PlayClipAtPoint(deathAudioClip, transform.position, 0.5f);
            }
        }
    }
}
