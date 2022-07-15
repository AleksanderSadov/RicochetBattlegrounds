using UnityEngine;
using UnityEngine.AI;

namespace Unity.Ricochet.Game
{
    public class SpawnManager : MonoBehaviour
    {
        [Tooltip("Spawn radius in units from the spawner location")]
        public int spawnRadius = 30;

        [Tooltip("Spawner default location")]
        public GameObject spawnDefaultPosition;

        public GameObject player;
        public GameObject enemy;

        private void Start()
        {
            WarpToRandomSpawnPosition(player);
            WarpToRandomSpawnPosition(enemy);
        }

        private void WarpToRandomSpawnPosition(GameObject entity)
        {
            Vector3 randomSpawnPosition;
            if (FindRandomSpawnPosition(spawnDefaultPosition.transform.position, spawnRadius, out randomSpawnPosition))
            {
                entity.GetComponent<NavMeshAgent>().Warp(randomSpawnPosition);
            }
        }

        private bool FindRandomSpawnPosition(Vector3 center, float range, out Vector3 result)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + new Vector3(
                    Random.Range(-range, range),
                    1.39f,
                    Random.Range(-range, range)
                );
                NavMeshHit hit;
                float maxHeight = 1.39f;
                float maxDistance = maxHeight;
                int walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");
                if (NavMesh.SamplePosition(randomPoint, out hit, maxDistance, walkableAreaMask))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }
    }
}
