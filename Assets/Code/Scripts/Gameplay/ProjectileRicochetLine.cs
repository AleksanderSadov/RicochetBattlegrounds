using UnityEngine;

namespace Unity.Ricochet.Gameplay
{
    public class ProjectileRicochetLine : MonoBehaviour
    {
        [SerializeField] GameObject gunPivot;

        private void Update()
        {
            Ray a = new Ray(gunPivot.transform.position, gunPivot.transform.forward);
            Ray b;
            RaycastHit hit;

            if (Deflect(a, out b, out hit))
            {
                Debug.DrawLine(a.origin, hit.point);
                Debug.DrawLine(b.origin, b.origin + 3 * b.direction);
            }
        }

        private bool Deflect(Ray ray, out Ray deflected, out RaycastHit hit)
        {

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 normal = hit.normal;
                Vector3 deflect = Vector3.Reflect(ray.direction, normal);

                deflected = new Ray(hit.point, deflect);
                return true;
            }

            deflected = new Ray(Vector3.zero, Vector3.zero);
            return false;
        }
    }
}

