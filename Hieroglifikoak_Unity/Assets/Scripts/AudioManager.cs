using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Sound[] soinuak;

    public static AudioManager instantzia;

	// Use this for initialization
	void Awake () {

        if (instantzia == null)
        {
            instantzia = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in soinuak)
        {
            s.source = gameObject.AddComponent<AudioSource>();

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            /*if (s.UI)
            {
                s.source.ignoreListenerPause = true;
            }*/
        }
	}

    void Start()
    {
        Play("Theme");
    }

    public void Play(string name)
    {
        bool soinuaAurkituta = false;
        foreach (Sound s in soinuak)
        {
            if(s.name == name)
            {
                if (!s.source.isPlaying || s.errepikatu)
                {
                    s.source.Play();
                }
                soinuaAurkituta = true;
                break;
            }
        }
        if(!soinuaAurkituta)
        {
            Debug.Log("Ez da " + name + " soinua aurkitu");
        }
    }

    public void Stop(string name)
    {
        bool soinuaAurkituta = false;
        foreach (Sound s in soinuak)
        {
            if (s.name == name)
            {
                s.source.Stop();
                soinuaAurkituta = true;
                break;
            }
        }
        if (!soinuaAurkituta)
        {
            Debug.Log("Ez da " + name + " soinua aurkitu");
        }
    }

    public void BotoiSoinua()
    {
        Play("Botoia");
    }
}
