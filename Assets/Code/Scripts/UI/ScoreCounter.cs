using TMPro;
using Unity.Ricochet.Game;
using UnityEngine;

namespace Unity.Ricochet.UI
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerScoreText;
        [SerializeField] private TextMeshProUGUI enemyScoreText;

        private void Update()
        {
            if (ScoreManager.Instance == null)
            {
                return;
            }

            playerScoreText.text = ScoreManager.Instance.playerScore.ToString();
            enemyScoreText.text = ScoreManager.Instance.enemyScore.ToString();
        }
    }
}
