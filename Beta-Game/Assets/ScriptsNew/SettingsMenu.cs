using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] AudioMixer Mixer;
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SfxSlider;
    public AudioSource MusicSource;
    public AudioClip[] clips;
 
    public const string Mixer_music = "Music";
    public const string Mixer_Sfx = "Sfx";

     void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat(AudioManager.Music_key, 1f);
        SfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_key, 1f);
        MusicSource.loop = false;

    }

    void Awake()
    {
        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        SfxSlider.onValueChanged.AddListener(SetSFXVolume);

    }
    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MucicPlayer.Music_key, MusicSlider.value);
        PlayerPrefs.SetFloat(MucicPlayer.SFX_key, SfxSlider.value);
    }
    public void Update()
    {
        if (!MusicSource.isPlaying)
        {
            MusicSource.clip = GetRandomClip();
            MusicSource.Play();
        }

    } 
    void SetMusicVolume(float value)
    {
    Mixer.SetFloat(Mixer_music, Mathf.Log10(value) * 20);
    }
    void SetSFXVolume(float value)
    {
    Mixer.SetFloat(Mixer_Sfx, Mathf.Log10(value) * 20);
    }
}
