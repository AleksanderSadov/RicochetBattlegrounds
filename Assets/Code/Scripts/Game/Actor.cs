using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class Actor : MonoBehaviour
    {
        public enum Affiliation { ENEMY, PLAYER };

        public Affiliation affiliation;

        private ActorsManager actorsManager;

        private void Start()
        {
            actorsManager = FindObjectOfType<ActorsManager>();

            if (!actorsManager.actors.Contains(this))
            {
                actorsManager.actors.Add(this);
            }
        }

        private void OnDestroy()
        {
            if (actorsManager)
            {
                actorsManager.actors.Remove(this);
            }
        }
    }
}
