using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class DeathTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.Kill();
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}
