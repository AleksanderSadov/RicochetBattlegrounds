using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class RayUtilities : MonoBehaviour
    {
        public static bool IsTargetInDirectSight(Transform sightOrigin, Transform target, float distance, int hitTestMask)
        {
            Vector3 sightPosition = sightOrigin.position;
            RaycastHit hit;
            Vector3 direction = (target.position - sightPosition).normalized;
            Physics.Raycast(sightPosition, direction, out hit, distance, hitTestMask);
            bool isTargetInDirectSight = hit.collider != null && target.gameObject == hit.collider.gameObject;

            return isTargetInDirectSight;
        }

        public static bool WillBulletHitOrigin(Transform origin, Transform target, float distance, int hitTestMask)
        {
            RaycastHit hit;
            Vector3 direction = target.forward;
            Physics.SphereCast(target.position, 0.75f, direction, out hit, distance, hitTestMask);
            bool willTargetHitOrigin = hit.collider != null && origin.gameObject == hit.collider.gameObject;

            return willTargetHitOrigin;
        }
    }
}

