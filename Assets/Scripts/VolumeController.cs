using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;

    void Start()
    {
        if (volumeSlider == null)
            volumeSlider = GetComponent<Slider>();
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
