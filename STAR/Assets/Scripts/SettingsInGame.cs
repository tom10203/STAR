using UnityEngine;
using UnityEngine.Audio;

public class SettingsInGame : MonoBehaviour
{
    void Start()
    {
        if (MenuManager.Instance != null)
        {
            //MenuManager.Instance.AssignSliders();
            //MenuManager.Instance.LoadVolume();
            //MenuManager.Instance.LoadMouse();
        }
        else
        {
            Debug.Log("no menumanager");
            this.gameObject.SetActive(false);
        }
    }
    #region Volume Functions

    public void ChangeMasterVolume()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.ChangeMasterVolume();
        }
    }
    public void ChangeMusicVolume()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.ChangeMusicVolume();
        }
    }
    public void ChangeSFXVolume()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.ChangeSFXVolume();
        }
    }

    #endregion
    #region Mouse Sensitivity Functions
    public void ChangeMouse()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.SaveMouse();
        }
    }
    public void LoadMouse()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.LoadMouse();
        }
    }
    #endregion
}
