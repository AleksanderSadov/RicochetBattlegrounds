using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class EnemyManager : MonoBehaviour
    {
        private int enemyCount = 0;

        public void EnemySpawned()
        {
            enemyCount++;
        }

        public void EnemyDied()
        {
            enemyCount--;

            if (enemyCount <= 0)
            {
                EventManager.Broadcast(new AllEnemiesKilled());
            }
        }
    }
}
