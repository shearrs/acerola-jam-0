using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("volumeSlider"))
            slider.value = PlayerPrefs.GetFloat("volumeSlider");
        else
        {
            slider.value = 1;
        }
    }

    public void UpdateVolume()
    {
        AudioListener.volume = slider.value;

        PlayerPrefs.SetFloat("volume", slider.value);
    }
}
