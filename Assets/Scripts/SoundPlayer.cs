using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip clip;
    public bool playOnAwake = true;

    public float volume = 1;
    public float pitchVariation = 0.1f;

    void Awake()
    {
        if(playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        var pitch = Random.Range(1-pitchVariation,1+pitchVariation);
        clip.Play(volume,pitch);
    }
}