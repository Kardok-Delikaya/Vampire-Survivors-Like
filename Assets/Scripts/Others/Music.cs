using UnityEngine;

namespace Others
{
    public class Music : MonoBehaviour
    {
        [SerializeField] private AudioClip musicClip;
        private AudioSource au;

        private void Start()
        {
            au = GetComponent<AudioSource>();
            Play(musicClip);
        }
        public void Play(AudioClip music)
        {
            au.clip = music;
            au.Play();
        }
    }
}