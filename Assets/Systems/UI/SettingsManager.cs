using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider mouseXSlider;
    [SerializeField] private Slider mouseYSlider;
    [SerializeField] private Slider gamepadXSlider;
    [SerializeField] private Slider gamepadYSlider;

    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("MasterVolume")) LoadMasterVolume();
        else SetMasterVolume();

        if (PlayerPrefs.HasKey("MusicVolume")) LoadMusicVolume();
        else SetMusicVolume();

        if (PlayerPrefs.HasKey("SFXVolume")) LoadSFXVolume();
        else SetSFXVolume();

        if (PlayerPrefs.HasKey("MouseXSens")) LoadMouseXSens();
        else SetPCXSensitivity();

        if (PlayerPrefs.HasKey("MouseYSens")) LoadMouseYSens();
        else SetPCYSensitivity();

        if (PlayerPrefs.HasKey("GamepadXSens")) LoadGamepadXSens();
        else SetGamepadXSensitivity();

        if (PlayerPrefs.HasKey("GamepadYSens")) LoadGamePadYSens();
        else SetGamepadYSensitivity();
    }

    #region Setting Functions

    private void LoadMasterVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        SetMasterVolume();
    }
    private void LoadMusicVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SetMusicVolume();
    }
    private void LoadSFXVolume()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        SetSFXVolume();
    }
    private void LoadMouseXSens()
    {
        mouseXSlider.value = PlayerPrefs.GetFloat("MouseXSens");
        SetPCXSensitivity();
    }
    private void LoadMouseYSens()
    {
        mouseYSlider.value = PlayerPrefs.GetFloat("MouseYSens");
        SetPCYSensitivity();
    }
    private void LoadGamepadXSens()
    {
        gamepadXSlider.value = PlayerPrefs.GetFloat("GamepadXSens");
        SetGamepadXSensitivity();
    }
    private void LoadGamePadYSens()
    {
        gamepadYSlider.value = PlayerPrefs.GetFloat("GamepadYSens");
        SetGamepadYSensitivity();
    }

    public void SetMasterVolume()
    {
        float volume = Mathf.Log10(masterSlider.value) * 20;
        audioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void SetMusicVolume()
    {
        float volume = Mathf.Log10(musicSlider.value) * 20;
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", masterSlider.value);
    }

    public void SetSFXVolume()
    {
        float volume = Mathf.Log10(sfxSlider.value) * 20;
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", masterSlider.value);
    }

    public void SetPCXSensitivity()
    {
        PlayerPrefs.SetFloat("MouseXSens", mouseXSlider.value);
    }

    public void SetPCYSensitivity()
    {
        PlayerPrefs.SetFloat("MouseYSens", mouseYSlider.value);
    }

    public void SetGamepadXSensitivity()
    {
        PlayerPrefs.SetFloat("GamepadXSens", gamepadXSlider.value);
    }

    public void SetGamepadYSensitivity()
    {
        PlayerPrefs.SetFloat("GamepadYSens", gamepadYSlider.value);
    }

    #endregion
}
