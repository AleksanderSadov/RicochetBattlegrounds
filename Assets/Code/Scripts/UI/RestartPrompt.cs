using TMPro;
using Unity.Ricochet.Game;
using UnityEngine;

namespace Unity.Ricochet.UI
{
    public class RestartPrompt : MonoBehaviour
    {
        [SerializeField] private GameObject restartText;

        private void Start()
        {
            EventManager.AddListener<GameOverEvent>(OnGameOver);
        }

        private void OnGameOver(GameOverEvent evt)
        {
            restartText.SetActive(true);
            restartText.transform.localScale = Vector3.one * 2.0f;
            iTween.Stop(restartText);
            iTween.ScaleTo(restartText, iTween.Hash("scale", Vector3.one, "time", 0.5f, "delay", 0.1f, "easetype", iTween.EaseType.spring));
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        }
    }
}

