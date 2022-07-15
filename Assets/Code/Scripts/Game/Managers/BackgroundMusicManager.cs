using System.Collections.Generic;
using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class BackgroundMusicManager : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> clipsList;
        [SerializeField] private AudioSource audioSource;

        public static BackgroundMusicManager Instance;

        private bool isPlayOnRepeat = false;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            isPlayOnRepeat = true;
        }

        private void Update()
        {
            if (isPlayOnRepeat && !audioSource.isPlaying)
            {
                PlayRandomClipOnce();
            }
        }

        public void PlayRandomClipOnce()
        {
            audioSource.clip = clipsList[Random.Range(0, clipsList.Count)];
            audioSource.Play();
        }

        public void PlayRandomclipsOnRepeat()
        {
            isPlayOnRepeat = true;
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }
    }
}

