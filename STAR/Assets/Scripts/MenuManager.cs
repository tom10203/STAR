using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public Slider volumeSlider; 
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [SerializeField] private AudioMixer audioMixer;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
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
}
