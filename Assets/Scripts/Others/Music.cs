using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Music : MonoBehaviour
    {
        [SerializeField] AudioClip musicClip;
        AudioSource au;
        void Start()
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