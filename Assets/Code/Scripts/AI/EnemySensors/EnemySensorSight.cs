using Unity.Ricochet.Game;
using UnityEngine;

namespace Unity.Ricochet.AI
{
    public class EnemySensorSight : MonoBehaviour
    {
        public float detectionAngle = 120.0f;
        public float detectionRange = 20.0f;
        public float attackRange = 20.0f;
        public float attackAngle = 20.0f;

        public LayerMask hitTestMask;
        public GameObject detectedTarget;
        public bool isSeeingTarget;
        public bool isTargetInAttackRange;
        public bool isTargetInAttackAngle;

        protected virtual void Start()
        {

        }

        protected virtual void FixedUpdate()
        {
            FindTargetInSight();
        }

        protected virtual void FindTargetInSight()
        {
            isSeeingTarget = false;
            isTargetInAttackRange = false;
            isTargetInAttackAngle = false;
            detectedTarget = null;

            Collider[] overlapedObjects = Physics.OverlapSphere(transform.position, detectionRange);
            for (int i = 0; i < overlapedObjects.Length; i++)
            {
                Vector3 direction = (overlapedObjects[i].transform.position - transform.position).normalized;
                float objAngle = Vector3.Angle(direction, transform.forward);
                if (overlapedObjects[i].CompareTag("Player"))
                {
                    if (objAngle < detectionAngle && RayUtilities.IsTargetInDirectSight(transform, overlapedObjects[i].transform, detectionRange, hitTestMask))
                    {
                        isSeeingTarget = true;
                        detectedTarget = overlapedObjects[i].transform.gameObject;
                        isTargetInAttackRange = Vector3.Distance(transform.position, detectedTarget.transform.position) <= attackRange;
                        isTargetInAttackAngle = objAngle < attackAngle;

                        return;
                    }
                }
            }
        }
    }
}
