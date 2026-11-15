using UnityEngine;
using UnityEngine.UI;

public class VolumenConfig : MonoBehaviour
{
    const string VolumePreferenceKey = "MasterVolume";

    [SerializeField] Scrollbar volumeScrollbar;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void LoadSavedVolume()
    {
        AudioListener.volume = Mathf.Clamp01(PlayerPrefs.GetFloat(VolumePreferenceKey, 1f));
    }

    void OnEnable()
    {
        if (volumeScrollbar == null)
        {
            volumeScrollbar = GetComponent<Scrollbar>();
        }

        if (volumeScrollbar == null)
        {
            Debug.LogError("VolumenConfig requires a Scrollbar reference.", this);
            enabled = false;
            return;
        }

        float savedVolume = Mathf.Clamp01(PlayerPrefs.GetFloat(VolumePreferenceKey, 1f));
        volumeScrollbar.SetValueWithoutNotify(savedVolume);
        volumeScrollbar.onValueChanged.AddListener(SetVolume);
        SetVolume(savedVolume);
    }

    void OnDisable()
    {
        if (volumeScrollbar != null)
        {
            volumeScrollbar.onValueChanged.RemoveListener(SetVolume);
        }

        PlayerPrefs.Save();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    void SetVolume(float volume)
    {
        float normalizedVolume = Mathf.Clamp01(volume);
        AudioListener.volume = normalizedVolume;
        PlayerPrefs.SetFloat(VolumePreferenceKey, normalizedVolume);
    }
}
