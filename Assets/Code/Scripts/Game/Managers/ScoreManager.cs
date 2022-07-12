using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class ScoreManager : MonoBehaviour
    {
        public int currentScore = 0;

        public void AddScore(int pointsAdded)
        {
            currentScore += pointsAdded;
        }
    }
}
