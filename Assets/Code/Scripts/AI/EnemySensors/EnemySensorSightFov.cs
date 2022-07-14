using UnityEngine;

namespace Unity.Ricochet.AI
{
    public class EnemySensorSightFov : EnemySensorSight
    {
        protected const float SIGHT_MIN_DISTANCE = 0.2f;

        public bool showFov = true;
        public Material fovMaterial;

        private int quality = 100;
        private Mesh mesh;

        protected override void Start()
        {
            base.Start();

            InitFov();
        }

        protected override void Update()
        {
            base.Update();

            if (!showFov)
            {
                return;
            }

            UpdateFov();
        }

        
        void InitFov()
        {
            mesh = new Mesh();
            mesh.vertices = new Vector3[2 * quality + 2];
            mesh.triangles = new int[3 * 2 * quality];

            Vector3[] normals = new Vector3[2 * quality + 2];
            Vector2[] uv = new Vector2[2 * quality + 2];

            for (int i = 0; i < uv.Length; i++)
            {
                uv[i] = new Vector2(0, 0);
            }    
                
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = new Vector3(0, 1, 0);
            }

            mesh.uv = uv;
            mesh.normals = normals;
            transform.Rotate(0.0f, 360 * -0.05f, 0.0f);
        }
        
        private void UpdateFov()
        {
            int bulletsLayerMask = 1 << LayerMask.NameToLayer("Bullets");
            int allExceptBulletsLayerMask = ~bulletsLayerMask;

            float angle_lookat = GetEnemyAngle();

            float angle_start = angle_lookat - detectionAngle;
            float angle_end = angle_lookat + detectionAngle;
            float angle_delta = (angle_end - angle_start) / quality;

            float angle_curr = angle_start;
            float angle_next = angle_start + angle_delta;

            Vector3 pos_curr_min = Vector3.zero;
            Vector3 pos_curr_max = Vector3.zero;

            Vector3 pos_next_min = Vector3.zero;
            Vector3 pos_next_max = Vector3.zero;

            Vector3[] vertices = new Vector3[4 * quality];   // Could be of size [2 * quality + 2] if circle segment is continuous
            int[] triangles = new int[3 * 2 * quality];
            float fov_height = 0.1f;
            Vector3 fov_position = transform.position;
            fov_position.y += fov_height;
            for (int i = 0; i < quality; i++)
            {

                Vector3 sphere_curr = new Vector3(
                    Mathf.Sin(Mathf.Deg2Rad * (angle_curr)), 0.0f,   // Left handed CW
                    Mathf.Cos(Mathf.Deg2Rad * (angle_curr)));

                Vector3 sphere_next = new Vector3(
                    Mathf.Sin(Mathf.Deg2Rad * (angle_next)), 0.0f,
                    Mathf.Cos(Mathf.Deg2Rad * (angle_next)));

                pos_curr_min = fov_position + sphere_curr * SIGHT_MIN_DISTANCE;
                pos_curr_max = fov_position + sphere_curr * detectionRange;
                pos_next_min = fov_position + sphere_next * SIGHT_MIN_DISTANCE;
                pos_next_max = fov_position + sphere_next * detectionRange;

                int a = 4 * i;
                int b = 4 * i + 1;
                int c = 4 * i + 2;
                int d = 4 * i + 3;

                RaycastHit currRay = new RaycastHit(), nextRay = new RaycastHit();
                Physics.Raycast(pos_curr_min, pos_curr_max - pos_curr_min, out currRay, detectionRange - SIGHT_MIN_DISTANCE, allExceptBulletsLayerMask);
                Physics.Raycast(pos_next_min, pos_next_max - pos_next_min, out nextRay, detectionRange - SIGHT_MIN_DISTANCE, allExceptBulletsLayerMask);

                if (currRay.collider != null)
                {
                    float dist = Vector3.Distance(currRay.point, pos_curr_min) + SIGHT_MIN_DISTANCE;
                    pos_curr_max = transform.position + sphere_curr * dist;
                }
                if (nextRay.collider != null)
                {
                    float dist = Vector3.Distance(nextRay.point, pos_next_min) + SIGHT_MIN_DISTANCE;
                    pos_next_max = transform.position + sphere_next * dist;
                }

                vertices[a] = pos_curr_min;
                vertices[b] = pos_curr_max;
                vertices[c] = pos_next_max;
                vertices[d] = pos_next_min;

                triangles[6 * i] = a;       // Triangle1: abc
                triangles[6 * i + 1] = b;
                triangles[6 * i + 2] = c;
                triangles[6 * i + 3] = c;   // Triangle2: cda
                triangles[6 * i + 4] = d;
                triangles[6 * i + 5] = a;

                angle_curr += angle_delta;
                angle_next += angle_delta;

            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, fovMaterial, 0);
        }

        private float GetEnemyAngle()
        {
            return 90 - Mathf.Rad2Deg * Mathf.Atan2(transform.forward.z, transform.forward.x); // Left handed CW. z = angle 0, x = angle 90
        }
    }
}
