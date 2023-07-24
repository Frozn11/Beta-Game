using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SondManeger : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    public float Volome = 1f;

        // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            Volome = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            volumeSlider.value = Volome / 1;
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();        
    }
    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("MusicVolume", volumeSlider.value);
    }

}
