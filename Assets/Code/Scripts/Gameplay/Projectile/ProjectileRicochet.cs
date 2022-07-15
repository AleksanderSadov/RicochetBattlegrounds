using Unity.Ricochet.Game;
using UnityEngine;

namespace Unity.Ricochet.Gameplay
{
    public class ProjectileRicochet : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 3.0f;
        [SerializeField] private float speed = 0.1f;
        [SerializeField] private float safeCollisionDelay = 0.1f;
        
        private float initializationTime;

        private void Start()
        {
            initializationTime = Time.timeSinceLevelLoad;
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.Translate(transform.forward * speed, Space.World);
        }

        private void OnCollisionEnter(Collision collision)
        {
            float bulletAliveTime = Time.timeSinceLevelLoad - initializationTime;
            if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")) && bulletAliveTime < safeCollisionDelay)
            {
                return;
            }

            Vector3 deflect = Vector3.Reflect(transform.forward, collision.contacts[0].normal);
            transform.forward = deflect;

            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Damageable>().Kill();
                Destroy(gameObject);
            }
        }
    }
}
