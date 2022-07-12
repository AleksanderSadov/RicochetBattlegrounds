using UnityEngine;

namespace Unity.Ricochet.Animations
{
    public class AnimRotateLoop : MonoBehaviour
    {
        public float time = 1.0f;
        public Vector3 angles = new Vector3();
        public iTween.EaseType easetype = iTween.EaseType.easeInOutSine;

        private void Start()
        {
            iTween.RotateTo(gameObject, iTween.Hash("rotation", angles, "time", time, "looptype", iTween.LoopType.pingPong, "easetype", easetype));
        }
    }
}

