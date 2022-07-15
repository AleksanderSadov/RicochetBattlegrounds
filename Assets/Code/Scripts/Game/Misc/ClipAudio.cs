using System.Collections.Generic;
using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class ClipAudio : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> clipsList;
        [SerializeField] private AudioSource audioSource;

        public void PlayRandomClipOnce()
        {
            audioSource.clip = clipsList[Random.Range(0, clipsList.Count)];
            audioSource.Play();
        }

        public void PlayRandomClipAtPointOnce(Vector3 point)
        {
            AudioSource.PlayClipAtPoint(clipsList[Random.Range(0, clipsList.Count)], point);
        }
    }
}

