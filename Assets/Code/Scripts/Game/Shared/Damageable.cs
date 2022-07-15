using UnityEngine;
using UnityEngine.Events;

namespace Unity.Ricochet.Game
{
    public class Damageable : MonoBehaviour
    {
        public UnityAction OnDie;

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
        }
    }
}
