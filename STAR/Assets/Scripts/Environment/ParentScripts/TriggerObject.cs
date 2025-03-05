using UnityEngine;

[RequireComponent (typeof(InteractableHandler))]
public class TriggerObject : MonoBehaviour
{
    [SerializeField] private InteractableHandler[] interactableObjects;
    public bool isTriggered; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
