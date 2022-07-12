using UnityEngine;

namespace Unity.Ricochet.Animations
{
    public class AnimColorTo : MonoBehaviour
    {
        public Color targetColor = Color.white;
        public float time = 2.0f;
        public float delay = 0.0f;
        public iTween.EaseType easetype;
        public iTween.LoopType looptype = iTween.LoopType.pingPong;

        private void Start()
        {
            iTween.ColorTo(gameObject, iTween.Hash("color", targetColor, "time", time, "delay", delay, "looptype", looptype, "easetype", easetype));
        }
    }
}
