using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

    public string name;

    public AudioClip clip;

    [Range(0,1)]
    public float volume;
    [Range(-3,3)]
    public float pitch;
    public bool loop;
    public bool UI;
    public bool errepikatu;

    [HideInInspector]
    public AudioSource source;
}
