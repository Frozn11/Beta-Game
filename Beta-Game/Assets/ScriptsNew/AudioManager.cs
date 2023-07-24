using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    [SerializeField] AudioMixer Mixer;
    [SerializeField] AudioSource GrapplingGusSource;
    [SerializeField] AudioSource crouchSource;
    [SerializeField] AudioSource walkingource;
    [SerializeField] List<AudioClip> crouchClip = new List<AudioClip>();
    [SerializeField] List<AudioClip> GrapplingGusClip = new List<AudioClip>();
    [SerializeField] List<AudioClip> walkingClip = new List<AudioClip>();


    public const string Music_key = "MusicVolume";
    public const string SFX_key = "SfxVolume";


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadVolume();
    }
    public void GrapplingGusSFX()
    {
        AudioClip clip1 = GrapplingGusClip[Random.Range(0, GrapplingGusClip.Count)];
        GrapplingGusSource.PlayOneShot(clip1);
    }
    public void crouchSFX()
    {
        AudioClip clip2 = crouchClip[Random.Range(0, crouchClip.Count)];
        crouchSource.PlayOneShot(clip2);

    }
    public void walkingSFX()
    {
        AudioClip clip2 = walkingClip[Random.Range(0, crouchClip.Count)];
        crouchSource.PlayOneShot(clip2);

    }

    void LoadVolume() //Volume seved on all sends
        {
            float musiceVolume = PlayerPrefs.GetFloat(Music_key, 1f);
            float sfxVolume = PlayerPrefs.GetFloat(SFX_key, 1f);

            Mixer.SetFloat(SettingsMenu.Mixer_music, Mathf.Log10(musiceVolume) * 20);
            Mixer.SetFloat(SettingsMenu.Mixer_Sfx, Mathf.Log10(sfxVolume) * 20);
        }

 }


