using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    [HideInInspector]
    public AudioSource musicSource;
    [HideInInspector]
    public List<AudioSource> sfxSources = new List<AudioSource>();

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
        DontDestroyOnLoad(this);
    }
    
    public void PlayMusic(AudioClip music)
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlayMusic(AudioClip music, bool playOnLoop)
    {   
        musicSource.loop = playOnLoop;
        PlayMusic(music);
    }

    public void PlaySFX(AudioClip sfx)
    {
        if (sfx != null)
        {
            AudioSource source = FindFirstSFXSourceEmpty();
            source.clip = sfx;
            source.Play();
        }
    }

    public void PlayRandomSFX(params AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            int randomIndex = Random.Range(0, clips.Length);
            AudioSource source = FindFirstSFXSourceEmpty();
            source.clip = clips[randomIndex];
            source.Play();
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

}
