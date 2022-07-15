using System.Collections.Generic;
using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class ClipAudio : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> clipsList;
        [SerializeField] private AudioSource audioSource;

        private bool isPlayOnRepeat = false;

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

        public void PlayRandomClipAtPointOnce(Vector3 point, float volume)
        {
            AudioSource.PlayClipAtPoint(clipsList[Random.Range(0, clipsList.Count)], point, volume);
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

