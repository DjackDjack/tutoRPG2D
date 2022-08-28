using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource src;

    public AudioClip townClip;
    public AudioClip adventure;



    public static AudioManager instance;

    private void Awake() 
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void ChangeSoundTrack(AudioClip myClip)
    {
        src.clip = myClip;
        src.Play();
    }

    public void Town()
    {
        src.clip = townClip;
        src.Play();
    }

    public void AdventurBegin()
    {
        src.clip = adventure;
        src.Play();
    }

}
