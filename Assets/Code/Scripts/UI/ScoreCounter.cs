using TMPro;
using Unity.Ricochet.Game;
using UnityEngine;

namespace Unity.Ricochet.UI
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private ScoreManager scoreManager;

        private void Start()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }

        private void Update()
        {
            scoreText.text = scoreManager.currentScore.ToString();
        }
    }
}
