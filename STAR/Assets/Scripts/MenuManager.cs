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

    private AudioSource buttonAudio;

    private PlayerCamera playerCam;
    //private SettingsInGame settingsInGame;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AssignSliders();
    }

    private void OnLevelWasLoaded(int level)
    {

        if (level != 0)
        {
            AssignSliders();
        }
       /* if (!PlayerPrefs.HasKey("masterVolume"))
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
            PlayerPrefs.SetFloat("mouseSensitivity", 0.1f); //just using template values here for now, feel free to change this and the slider settings in the inspector based on how it feels in-game
        }
        LoadMouse();

        buttonAudio = GetComponent<AudioSource>();*/
    }
    #region Volume Functions
    public void ChangeMasterVolume()
    {
        SaveMasterVolume();
    }
    public void ChangeMusicVolume()
    {
        SaveMusicVolume();
    }
    public void ChangeSFXVolume()
    {
        SaveSFXVolume();
    }

    public void LoadMasterVolume()
    {
        Debug.Log("LoadMasterVolume");
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume", 0.8f);
        audioMixer.SetFloat("MasterVol", ((100 * volumeSlider.value) - 80f));
    }

    public void LoadMusicVolume()
    {
        Debug.Log("LoadMusicVolume");
        
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolume", 0.8f);
        audioMixer.SetFloat("MusicVol", ((100 * musicVolumeSlider.value) - 80f));
    }

    public void LoadSFXVolume()
    {
        Debug.Log("LoadSFXVolume");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume", 0.8f);
        audioMixer.SetFloat("SFXVol", ((100 * sfxVolumeSlider.value) - 80f));
    }

    public void SaveMasterVolume()
    {
        Debug.Log("SaveMasterVolume");
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
        audioMixer.SetFloat("MasterVol", ((100 * volumeSlider.value) - 80f));
    }

    public void SaveMusicVolume()
    {
        Debug.Log("SaveMusicVolume");
        PlayerPrefs.SetFloat("musicVolume", musicVolumeSlider.value);
        audioMixer.SetFloat("MusicVol", ((100 * musicVolumeSlider.value) - 80f));
    }

    public void SaveSFXVolume()
    {
        Debug.Log("SaveSFXVolume");
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
        mouseSlider.value = PlayerPrefs.GetFloat("mouseSensitivity", 0.1f);
    }
    public void SaveMouse()
    {
        PlayerPrefs.SetFloat("mouseSensitivity", mouseSlider.value);
        if (playerCam != null)
        {
            playerCam.SetMouseSensitivity(mouseSlider.value);
        }
    }
    #endregion
    #region Level Buttons
    //This probably isn't the ideal way of doing this, but it's quick and easy!
    public void Level1()
    {
        buttonAudio.PlayOneShot(buttonAudio.clip);
        SceneManager.LoadScene(1);
    }
    public void Level2()
    {
        buttonAudio.PlayOneShot(buttonAudio.clip);
        SceneManager.LoadScene(2);
    }
    public void Level3()
    {
        buttonAudio.PlayOneShot(buttonAudio.clip);
        SceneManager.LoadScene(3);
    }
    public void Level4()
    {
        buttonAudio.PlayOneShot(buttonAudio.clip);
        SceneManager.LoadScene(4);
    }
    #endregion

    public void AssignSliders()
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
        //LoadVolume();
        #endregion
        #region Mouse Sensitivity Initialise
        if (!PlayerPrefs.HasKey("mouseSensitivity"))
        {
            PlayerPrefs.SetFloat("mouseSensitivity", 0.1f); //just using template values here for now, feel free to change this and the slider settings in the inspector based on how it feels in-game
        }
        //LoadMouse();
        #endregion

        buttonAudio = GetComponent<AudioSource>();
        GameObject playerCameraObj = GameObject.FindWithTag("PlayerCamera");
        if (playerCameraObj)
        {
            playerCam = playerCameraObj.GetComponent<PlayerCamera>();
        }
        volumeSlider = GameObject.FindWithTag("VolumeSlider").GetComponent<Slider>();
        musicVolumeSlider = GameObject.FindWithTag("MusicVolumeSlider").GetComponent<Slider>();
        sfxVolumeSlider = GameObject.FindWithTag("SFXVolumeSlider").GetComponent<Slider>();
        mouseSlider = GameObject.FindWithTag("MouseSlider").GetComponent<Slider>();
        //mouseSlider.transform.parent.gameObject.SetActive(false);

        LoadMasterVolume();
        LoadMusicVolume();
        LoadSFXVolume();
        LoadMouse();

        Debug.Log("AssignSliders Called");

        mouseSlider.transform.parent.gameObject.SetActive(false);
        /*settingsInGame = FindAnyObjectByType<SettingsInGame>();
        if (settingsInGame != null)
        {
            settingsInGame.gameObject.SetActive(false);
        }*/
    }
}
