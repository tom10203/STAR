using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [Header("Volume")]
    public Slider volumeSlider; 
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;

    [Header("Mouse Sensitivity")]
    public Slider mouseSlider;
    //Must remember to make sure the mouse sensitivity variable in the player script is set to PlayerPrefs.GetFloat("mouseSensitivity") in its start function

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        #region Volume Initialise
        if (!PlayerPrefs.HasKey("masterVolume"))
        {
            PlayerPrefs.SetFloat("masterVolume", 0.8f);
        }
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.8f);
        }
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 0.8f);
        }
        LoadVolume();
        #endregion
        #region Mouse Sensitivity Initialise
        if (!PlayerPrefs.HasKey("mouseSensitivity"))
        {
            PlayerPrefs.SetFloat("mouseSensitivity", 0.5f); //just using template values here for now, feel free to change this and the slider settings in the inspector based on how it feels in-game
        }
        LoadMouse();
        #endregion
    }
    #region Volume Functions
    public void ChangeVolume()
    {
        SaveVolume();
    }
    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        audioMixer.SetFloat("MasterVol", ((100 * volumeSlider.value) - 80f));
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        audioMixer.SetFloat("MusicVol", ((100 * musicVolumeSlider.value) - 80f));
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        audioMixer.SetFloat("SFXVol", ((100 * sfxVolumeSlider.value) - 80f));
    }
    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
        audioMixer.SetFloat("MasterVol", ((100 * volumeSlider.value) - 80f));
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        audioMixer.SetFloat("MusicVol", ((100 * musicVolumeSlider.value) - 80f));
        PlayerPrefs.SetFloat("sfxVolume", sfxVolumeSlider.value);
        audioMixer.SetFloat("SFXVol", ((100 * sfxVolumeSlider.value) - 80f));
    }
    #endregion
    #region Mouse Sensitivity Functions
    public void ChangeMouse()
    {
        SaveMouse();
    }
    public void LoadMouse()
    {
        mouseSlider.value = PlayerPrefs.GetFloat("mouseSensitivity");
    }
    public void SaveMouse()
    {
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSlider.value);
    }
    #endregion
    #region Level Buttons
    //This probably isn't the ideal way of doing this, but it's quick and easy!
    public void Level1()
    {
        SceneManager.LoadScene(1);
    }
    public void Level2()
    {
        SceneManager.LoadScene(2);
    }
    public void Level3()
    {
        SceneManager.LoadScene(3);
    }
    public void Level4()
    {
        SceneManager.LoadScene(4);
    }
    #endregion
}
