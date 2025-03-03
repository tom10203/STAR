using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public Slider volumeSlider;

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
            LoadVolume();
        }
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
        AudioListener.volume = volumeSlider.value;
        SaveVolume();
    }
    public void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        audioMixer.SetFloat("MasterVol", ((100 * volumeSlider.value) - 80f));
    }
    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", volumeSlider.value);
        audioMixer.SetFloat("MasterVol", ((100 * volumeSlider.value) -80f));
    }
}
