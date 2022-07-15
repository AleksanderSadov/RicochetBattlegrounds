using Unity.Ricochet.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.Ricochet.AI
{
    public class EnemySensorDanger : MonoBehaviour
    {
        public float dangerCheckProximity = 10f;

        public UnityAction onDangerDetected;

        public bool isDangerDetected;
        public GameObject detectedBullet;

        private void FixedUpdate()
        {
            CheckBulletsInProximity();
        }

        private void CheckBulletsInProximity()
        {
            isDangerDetected = false;
            detectedBullet = null;

            Collider[] overlapedObjects = Physics.OverlapSphere(transform.position, dangerCheckProximity);
            for (int i = 0; i < overlapedObjects.Length; i++)
            {
                if (overlapedObjects[i].CompareTag("Bullet")
                    && RayUtilities.IsTargetInDirectSight(transform, overlapedObjects[i].transform, dangerCheckProximity, LayerMask.GetMask("Bullets"))
                    && RayUtilities.WillBulletHitOrigin(transform, overlapedObjects[i].transform, dangerCheckProximity, LayerMask.GetMask("Default")))
                {
                    isDangerDetected = true;
                    detectedBullet = overlapedObjects[i].transform.gameObject;
                    onDangerDetected?.Invoke();

                    return;
                }
            }
        }
    }
}

