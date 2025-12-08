using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    #region Setting Functions

    public void SetMasterVolume(float volume)
    {
        Debug.Log($"New Master Volume: {volume}");
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log($"New Music Volume: {volume}");
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log($"New SFX Volume: {volume}");
    }

    public void SetPCXSensitivity(float sensitivity)
    {
        Debug.Log($"New PC X Sens: {sensitivity}");
    }

    public void SetPCYSensitivity(float sensitivity)
    {
        Debug.Log($"New PC Y Sens: {sensitivity}");
    }

    public void SetGamepadXSensitivity(float sensitivity)
    {
        Debug.Log($"New Gamepad X Sens: {sensitivity}");
    }

    public void SetGamepadYSensitivity(float sensitivity)
    {
        Debug.Log($"New Gamepad Y Sens: {sensitivity}");
    }

    #endregion
}
