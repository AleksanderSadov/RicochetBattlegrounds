using UnityEngine;
using UnityEngine.Events;

namespace Unity.Ricochet.Game
{
    public class Damageable : MonoBehaviour
    {
        public UnityAction OnDie;

        public void Kill()
        {
            //OnDie?.Invoke();
        }
    }
}
