using System.Collections.Generic;
using UnityEngine;

namespace Unity.Ricochet.Game
{
    public class GunShotsAudio : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> shotsList;
        [SerializeField] private AudioSource shotSource;

        public void PlayGunShot()
        {
            shotSource.clip = shotsList[Random.Range(0, shotsList.Count)];
            shotSource.Play();
        }
    }
}

