using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    AudioManager audioManager;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider mouseXSlider;
    [SerializeField] private Slider mouseYSlider;
    [SerializeField] private Slider gamepadXSlider;
    [SerializeField] private Slider gamepadYSlider;

    private float masterSliderValue;
    private float musicSliderValue;
    private float sfxSliderValue;
    private float mouseXSliderValue;
    private float mouseYSliderValue;
    private float gamepadXSliderValue;
    private float gamepadYSliderValue;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
    }

    public void OnOpen()
    {
        masterSlider.value = masterSliderValue;
        musicSlider.value = musicSliderValue;
        sfxSlider.value = sfxSliderValue;
        mouseXSlider.value = mouseXSliderValue;
        mouseYSlider.value = mouseYSliderValue;
        gamepadXSlider.value = gamepadXSliderValue;
        gamepadYSlider.value = gamepadYSliderValue;
    }

    private void OnEnable()
    {
        LoadPrefs();
    }

    #region Setting Functions

    private void LoadPrefs()
    {
        masterSliderValue = PlayerPrefs.GetFloat("MasterVolume");
        musicSliderValue = PlayerPrefs.GetFloat("MusicVolume");
        sfxSliderValue = PlayerPrefs.GetFloat("SFXVolume");
        mouseXSliderValue = PlayerPrefs.GetFloat("MouseXSens");
        mouseYSliderValue = PlayerPrefs.GetFloat("MouseYSens");
        gamepadXSliderValue = PlayerPrefs.GetFloat("GamepadXSens");
        gamepadYSliderValue = PlayerPrefs.GetFloat("GamepadYSens");
    }

    public void SetMasterVolume(float volume)
    {
        audioManager.DefaultAudioMixer.SetFloat("MasterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioManager.DefaultAudioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioManager.DefaultAudioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetPCXSensitivity(float sensitivity)
    {
        Debug.Log($"New PC X Sens: {sensitivity}");
        PlayerPrefs.SetFloat("MouseXSens", sensitivity);
    }

    public void SetPCYSensitivity(float sensitivity)
    {
        Debug.Log($"New PC Y Sens: {sensitivity}");
        PlayerPrefs.SetFloat("MouseYSens", sensitivity);
    }

    public void SetGamepadXSensitivity(float sensitivity)
    {
        Debug.Log($"New Gamepad X Sens: {sensitivity}");
        PlayerPrefs.SetFloat("GamepadXSens", sensitivity);
    }

    public void SetGamepadYSensitivity(float sensitivity)
    {
        Debug.Log($"New Gamepad Y Sens: {sensitivity}");
        PlayerPrefs.SetFloat("GamepadYSens", sensitivity);
    }

    #endregion
}
