using UnityEngine;

namespace Unity.Ricochet.AI
{
    public class EnemySensorBase : MonoBehaviour
    {
        public Enemy enemy;

        protected virtual void Start()
        {
            if (enemy == null)
            {
                enemy = gameObject.GetComponent<Enemy>();
            }
        }

        protected virtual void Update()
        {

        }
    }
}
