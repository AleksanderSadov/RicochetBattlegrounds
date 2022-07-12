using UnityEngine;

namespace Unity.Ricochet.Animations
{
    public class AnimLevitate : MonoBehaviour
    {
        public Vector2 time;
        public Vector2 direction;
        public iTween.EaseType easetype = iTween.EaseType.easeInOutSine;
        public bool isLocal = false;

        private void Start()
        {
            Vector3 targetPosition = transform.position;
            targetPosition.x += direction.x;
            targetPosition.y += direction.y;
            iTween.MoveTo(gameObject, iTween.Hash("x", targetPosition.y, "time", time.y, "looptype", iTween.LoopType.pingPong, "easetype", easetype, "islocal", isLocal));
        }
    }
}
