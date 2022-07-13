using UnityEngine;

namespace Unity.Ricochet.AI
{
    public class EnemySensorSight : EnemySensorBase
    {
        protected const float SIGHT_DIRECT_ANGLE = 120.0f, SIGHT_MIN_DISTANCE = 0.2f, SIGHT_MAX_DISTANCE = 20.0f;

        float height = 2.0f;
        public LayerMask hitTestMask;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();

            FindTargetInSight();
        }

        private void FindTargetInSight()
        {
            Collider[] overlapedObjects = Physics.OverlapSphere(transform.position, SIGHT_MAX_DISTANCE);

            for (int i = 0; i < overlapedObjects.Length; i++)
            {
                Vector3 direction = overlapedObjects[i].transform.position - transform.position;
                float objAngle = Vector3.Angle(direction, transform.forward);
                if (overlapedObjects[i].tag == "Player")
                {
                    if (objAngle < SIGHT_DIRECT_ANGLE && IsTargetInSight(overlapedObjects[i].transform, SIGHT_MAX_DISTANCE))
                    {
                        enemy.SetTargetPos(overlapedObjects[i].transform.position);
                    }
                }
            }
        }

        private bool IsTargetInSight(Transform target, float distance)
        {
            Vector3 sightPosition = transform.position;
            sightPosition.y += height;
            RaycastHit hit = new RaycastHit();
            Vector3 dir = target.position - sightPosition;
            Physics.Raycast(sightPosition, dir, out hit, distance, hitTestMask);
            return (hit.collider != null && target.gameObject == hit.collider.gameObject);
        }
    }
}
