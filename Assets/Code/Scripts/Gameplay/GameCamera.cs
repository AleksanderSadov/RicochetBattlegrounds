using System.Collections;
using UnityEngine;

namespace Unity.Ricochet.Gameplay
{
    public class GameCamera : MonoBehaviour
    {
        public Transform trackedObject;
        public Transform targetCamera;

        private float shakeDelay = 0.03f;
        private float lastShakeTime = float.MinValue;
        private bool isShaking = false;

        private void Update()
        {
            TrackObject();
            HandleShake();
        }

        public void ToggleShake(float shakeTime)
        {
            isShaking = true;
            StartCoroutine(WaitShakeFinish(shakeTime));
        }

        private void TrackObject()
        {
            targetCamera.position = Vector3.Lerp(targetCamera.position, trackedObject.position, 0.05f);
        }

        private void HandleShake()
        {
            if (!isShaking)
            {
                return;
            }

            if (lastShakeTime + shakeDelay < Time.time)
            {
                Vector3 shakePosition = Vector3.zero;
                shakePosition.x += Random.Range(-0.5f, 0.5f);
                shakePosition.y += Random.Range(-0.5f, 0.5f);
                targetCamera.transform.Translate(shakePosition);
                lastShakeTime = Time.time;
            }
        }

        private IEnumerator WaitShakeFinish(float shakeTime)
        {
            yield return new WaitForSeconds(shakeTime);
            isShaking = false;
        }
    }
}
