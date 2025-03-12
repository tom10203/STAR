using UnityEngine;

public class InGameButtons : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject objectToDeactivate;
    private InGameUI inGameUI;
    private AudioSource audio;
    private bool started = false;

    public void Interact()
    {
        if (!started)
        {
            started = true;
            Debug.Log("start!");
            objectToDeactivate.SetActive(false);
            inGameUI = FindAnyObjectByType<InGameUI>();
            inGameUI.StartTimer();

            audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audio.clip);
        }
    }
}
