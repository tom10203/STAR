using UnityEngine;
using UnityEngine.Audio;

public class SettingsInGame : MonoBehaviour
{
    void Start()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.AssignSliders();
            MenuManager.Instance.LoadVolume();
            MenuManager.Instance.LoadMouse();
        }
        this.gameObject.SetActive(false);
    }
    #region Volume Functions
    public void ChangeVolume()
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.SaveVolume();
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
