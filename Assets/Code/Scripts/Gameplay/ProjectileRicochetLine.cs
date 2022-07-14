using UnityEngine;

namespace Unity.Ricochet.Gameplay
{
    public class ProjectileRicochetLine : MonoBehaviour
    {
        [SerializeField] private GameObject gunPivot;
        [SerializeField] private int maxDeflectsCalculations = 1;
        [SerializeField] private int currentDeflectsCount = 0;

        private void Update()
        {
            Ray originRay = new Ray(gunPivot.transform.position, gunPivot.transform.forward);
            Ray deflectedRay;
            RaycastHit deflectHit;

            bool isDeflected;
            currentDeflectsCount = 0;
            do
            {
                isDeflected = IsDeflected(originRay, out deflectedRay, out deflectHit);

                if (isDeflected)
                {
                    currentDeflectsCount++;
                    Debug.DrawLine(originRay.origin, deflectHit.point);
                    Debug.DrawLine(deflectedRay.origin, deflectedRay.origin + 3 * deflectedRay.direction);
                    originRay = deflectedRay;
                }
                else
                {
                    Debug.DrawLine(originRay.origin, originRay.origin * 10);
                }
            }
            while (isDeflected && currentDeflectsCount < maxDeflectsCalculations);
        }

        private bool IsDeflected(Ray originRay, out Ray deflectedRay, out RaycastHit deflectHit)
        {

            if (Physics.Raycast(originRay, out deflectHit))
            {
                Vector3 normal = deflectHit.normal;
                Vector3 deflect = Vector3.Reflect(originRay.direction, normal);

                deflectedRay = new Ray(deflectHit.point, deflect);
                return true;
            }

            deflectedRay = new Ray(Vector3.zero, Vector3.zero);
            return false;
        }
    }
}

