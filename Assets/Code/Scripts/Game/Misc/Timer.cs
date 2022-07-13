using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class Timer : MonoBehaviour
    {
        public string timerName;
        public bool isActive { get; private set; } = false;
        public bool isFinished { get; private set; } = false;
        public float timeElapsed { get; private set; } = 0;
        public float duration { get; private set; } = 0;

        private void Update() {
            UpdateTimer();
        }

        public void StartTimer(float timerDuration)
        {
            if (timerDuration < 0.0f)
            {
                isFinished = true;
                isActive = false;
            }
            else
            {
                timeElapsed = 0.0f;
                duration = timerDuration;
                isFinished = false;
                isActive = true;
            }
        }

        public void StopTimer()
        {
            timeElapsed = 0.0f;
            isFinished = false;
            isActive = false;
        }

        private void UpdateTimer()
        {
            if (!isActive)
            {
                return;
            }

            timeElapsed += Time.deltaTime;
            if (timeElapsed > duration)
            {
                isActive = false;
                isFinished = true;
            }
        }
    }
}
