using UnityEngine;

public class InGameButtons : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject objectToDeactivate;
    private InGameUI inGameUI;
    public void Interact()
    {
        Debug.Log("start!");
        objectToDeactivate.SetActive(false);
        inGameUI = FindAnyObjectByType<InGameUI>();
        inGameUI.StartTimer();
    }

}
