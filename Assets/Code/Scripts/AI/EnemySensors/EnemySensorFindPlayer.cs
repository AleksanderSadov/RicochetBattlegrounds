using UnityEngine;

namespace Unity.Ricochet.AI
{
    public class EnemySensorFindPlayer : MonoBehaviour
    {
        public GameObject player;
        public Vector3 playerPosition;

        private void Start()
        {
            ActorsManager actorsManager = FindObjectOfType<ActorsManager>();
            if (actorsManager != null)
            {
                player = actorsManager.player;
            }
        }

        private void Update()
        {
            if (player != null)
            {
                playerPosition = player.transform.position;
            }
        }
    }
}

