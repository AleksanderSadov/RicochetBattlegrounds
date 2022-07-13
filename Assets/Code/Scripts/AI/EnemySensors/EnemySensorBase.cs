using System.Collections.Generic;
using UnityEngine;

namespace Unity.Ricochet.AI
{
    public class EnemySensorBase : MonoBehaviour
    {
        public Enemy enemy;
        protected List<GameObject> sensedObjects = new List<GameObject>();

        private void Start()
        {
            if (enemy == null)
            {
                enemy = gameObject.GetComponent<Enemy>();
            }

            StartSensor();
        }

        void Update()
        {
            UpdateSensor();
        }

        protected virtual void StartSensor() { }
        protected virtual void UpdateSensor() { }

        protected List<GameObject> GetSensedObjects()
        {
            return sensedObjects;
        }
    }
}
