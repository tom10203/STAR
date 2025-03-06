using UnityEngine;

public class InGameButtons : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject objectToDeactivate;
    public void Interact()
    {
        Debug.Log("start!");
        objectToDeactivate.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
