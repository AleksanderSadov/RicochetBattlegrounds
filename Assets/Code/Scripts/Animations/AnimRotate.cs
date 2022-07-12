using UnityEngine;

namespace Unity.Ricochet.Animations
{
    public class AnimRotate : MonoBehaviour
    {
        public float rotateLoopTime = 1.0f;
        public bool rotateLeft = true;
        public iTween.EaseType easetype = iTween.EaseType.linear;
        public float addValue = 360;

        private void OnEnable()
        {
            if (rotateLeft)
            {
                iTween.RotateAdd(gameObject, iTween.Hash("z", addValue, "time", rotateLoopTime, "easetype", easetype, "oncomplete", "DoAction"));
            }
            else
            {
                iTween.RotateAdd(gameObject, iTween.Hash("z", -addValue, "time", rotateLoopTime, "easetype", easetype, "oncomplete", "DoAction"));
            }
        }

        private void OnDisable()
        {
            iTween.Stop(gameObject);
        }
    }
}
