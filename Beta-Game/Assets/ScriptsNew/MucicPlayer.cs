using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MucicPlayer : MonoBehaviour
{

    [SerializeField] AudioMixer Mixer;
    public static MucicPlayer instance;
    public AudioClip[] clips;
    public AudioSource audiocSource;

    public const string Music_key = "MusicVolume";
    public const string SFX_key = "SfxVolume";



    // Start is called before the first frame update
    void Start()
    {

        audiocSource.loop = false;

    }
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
    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }



    void LoadVolume() //Volume seved on all sends
    {
        float musiceVolume = PlayerPrefs.GetFloat(Music_key, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_key, 1f);

        Mixer.SetFloat(SettingsMenu.Mixer_music, Mathf.Log10(musiceVolume) * 0);
        Mixer.SetFloat(SettingsMenu.Mixer_Sfx, Mathf.Log10(sfxVolume) * 0);
    }


    private void Update()
    {
        if (!audiocSource.isPlaying)
        {
            audiocSource.clip = GetRandomClip();
            audiocSource.Play();
        }
    }
}
