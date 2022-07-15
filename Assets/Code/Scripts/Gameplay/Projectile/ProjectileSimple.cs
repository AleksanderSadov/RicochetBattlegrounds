using Unity.Ricochet.Game;
using UnityEngine;

namespace Unity.Ricochet.Gameplay
{
    public class ProjectileSimple : MonoBehaviour
    {
        public float lifeTime = 3.0f;
        public float speed = 1.5f;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void FixedUpdate()
        {
            transform.Translate(transform.forward * speed, Space.World);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Damageable>().Kill();
            }

            Destroy(gameObject);
        }
    }
}
