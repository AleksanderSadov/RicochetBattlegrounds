using UnityEngine;

namespace Unity.Ricochet.Gameplay
{
    public class ProjectileRicochetLine : MonoBehaviour
    {
        [SerializeField] private GameObject gunPivot;
        [SerializeField] private GameObject mousePointer;
        [SerializeField] private Material ricochetLineMaterial;
        [SerializeField] private int maxDeflectsCalculations = 1;
        [SerializeField] private int currentDeflectsCount = 0;

        private LineRenderer lineRenderer;

        private void Start()
        {
            InitLineRenderer();
        }

        private void Update()
        {
            Ray originRay = new Ray(gunPivot.transform.position, mousePointer.transform.position - gunPivot.transform.position);
            Ray deflectedRay;
            RaycastHit deflectHit;

            bool isDeflected;
            currentDeflectsCount = 0;
            lineRenderer.positionCount = 0;
            do
            {
                isDeflected = IsDeflected(originRay, out deflectedRay, out deflectHit);

                if (isDeflected)
                {
                    currentDeflectsCount++;
                    SetNextDrawLinePositions(originRay.origin, deflectedRay.origin);

                    if (currentDeflectsCount == maxDeflectsCalculations)
                    {
                        SetNextDrawLinePositions(deflectedRay.origin + deflectedRay.direction * 3);
                    }

                    originRay = deflectedRay;
                }
                else
                {
                    SetNextDrawLinePositions(originRay.origin, originRay.origin + originRay.direction * 30);
                }
            }
            while (isDeflected && currentDeflectsCount < maxDeflectsCalculations);
        }

        private void OnDisable()
        {
            lineRenderer.positionCount = 0;
        }

        private bool IsDeflected(Ray originRay, out Ray deflectedRay, out RaycastHit deflectHit)
        {
            int bulletsLayerMask = 1 << LayerMask.NameToLayer("Bullets");
            int allExceptBulletsLayerMask = ~bulletsLayerMask;

            if (Physics.Raycast(originRay, out deflectHit, Mathf.Infinity, allExceptBulletsLayerMask))
            {
                Vector3 normal = deflectHit.normal;
                Vector3 deflect = Vector3.Reflect(originRay.direction, normal);

                deflectedRay = new Ray(deflectHit.point, deflect);
                return true;
            }

            deflectedRay = new Ray(Vector3.zero, Vector3.zero);
            return false;
        }

        private void InitLineRenderer()
        {
            GameObject ricochetLine = new GameObject();
            ricochetLine.transform.parent = gameObject.transform;
            ricochetLine.transform.position = gunPivot.transform.position;
            ricochetLine.name = "RicochetLine";
            lineRenderer = ricochetLine.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.material = ricochetLineMaterial;
        }

        private void SetNextDrawLinePositions(params Vector3[] positions)
        {
            foreach(Vector3 position in positions)
            {
                int lastPositionIndex = lineRenderer.positionCount - 1;
                if (lastPositionIndex > - 1
                    && lineRenderer.GetPosition(lastPositionIndex).Equals(position))
                {
                    continue;
                }

                int nextPositionIndex = lineRenderer.positionCount;
                lineRenderer.positionCount = nextPositionIndex + 1;
                lineRenderer.SetPosition(nextPositionIndex, position);
            }
        }
    }
}

