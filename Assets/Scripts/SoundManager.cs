using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    public AudioClip titleMusic;
    public AudioClip bossMusic;
    public AudioClip theme1Music;

    public int currentMusic = 0;

    [HideInInspector]
    public AudioSource musicSource;
    [HideInInspector]
    public List<AudioSource> sfxSources = new List<AudioSource>();

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    [HideInInspector]
    public static SoundManager instance = null;

	// Use this for initialization
	void Awake () {
        if(instance == null) 
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        var audioSources = gameObject.GetComponents<AudioSource>();
        musicSource = audioSources[0];
        for (int i = 1; i < audioSources.Length; i++)
            sfxSources.Add(audioSources[i]);
    }

    public void PlayMusic(AudioClip music, float fadeDuration = 0)
    {
        if (music != null)
        {
            if (!musicSource.isPlaying)
            {
                musicSource.clip = music;
                musicSource.Play();
            }
            else
            {
                Debug.Log("ici");
                if(fadeDuration == 0)
                {
                    musicSource.Stop();
                    musicSource.clip = music;
                    musicSource.Play();
                }
                else
                {
                    Debug.Log("la");
                    StartCoroutine(this.MusicTransition(musicSource, music, fadeDuration));
                }
            }
        }
    }

    public void PlayMusic(AudioClip music, bool playOnLoop, float fadeDuration = 0)
    {   
        musicSource.loop = playOnLoop;
        PlayMusic(music, fadeDuration);
    }

    public void PlaySFX(AudioClip sfx)
    {
        if (sfx != null)
        {
            AudioSource source = FindFirstSFXSourceEmpty();
            if (source != null)
            {
                source.clip = sfx;
                source.Play();
            }
        }
    }

    public void PlayRandomSFX(params AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            AudioSource source = FindFirstSFXSourceEmpty();
            if (source != null)
            {
                source.pitch = randomPitch;
                source.clip = clips[randomIndex];
                source.Play(); 
            }
        }
    }

    private AudioSource FindFirstSFXSourceEmpty()
    {
    
        foreach(AudioSource source in sfxSources)
        {
            if (!source.isPlaying)
                return source;
        }
        return null;
    }

    private IEnumerator MusicTransition(AudioSource source, AudioClip newMusic, float fadeTime)
    {
        float initialVolume = source.volume;
        while (source.volume > 0)
        {
            source.volume -= initialVolume * Time.deltaTime / (fadeTime / 2f);
            yield return null;
        }
        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.Play();
        while(source.volume < initialVolume)
        {
            source.volume += Time.deltaTime / (fadeTime / 2f);
            yield return null;
        }
    }

}
