using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public GameObject bgMusicGO;
    public GameObject sfxGO;
    public GameObject sfx2GO;

    private AudioSource bgAudiosource;
    private AudioSource sfxAudiosource;
    private AudioSource sfx2Audiosource;



    void Start()
    {
        Instance = this.GetComponent<AudioManager>();
        bgAudiosource = bgMusicGO.GetComponent<AudioSource>();
        sfxAudiosource = sfxGO.GetComponent<AudioSource>();
        sfx2Audiosource = sfx2GO.GetComponent<AudioSource>();
    }

        // Update is called once per frame
        void Update()
    {
        
    }

    public void PlaySFXsound(AudioClip sound)
    {
        sfxAudiosource.clip = sound;
        sfxAudiosource.Play();
    }

    public void PlaySFX2sound(AudioClip sound)
    {
        sfxAudiosource.clip = sound;
        sfxAudiosource.Play();
    }
}
