using UnityEngine;

public class Audio : Singleton<Audio>
{
    AudioSource source;

    void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
    }


    public void Play(AudioClip clip, float volume = 1, float pitch = 1)
    {
        source.pitch = pitch;
        source.PlayOneShot(clip,volume);
    }
}

public static class AudioExtensions
{
    public static void Play(this AudioClip clip, float volume = 1, float pitch = 1)
    {
        Audio.Instance.Play(clip, volume,pitch);
    }
}