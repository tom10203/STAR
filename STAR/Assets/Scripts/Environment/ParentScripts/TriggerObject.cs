using UnityEngine;


public class TriggerObject : MonoBehaviour
{
    [SerializeField] private InteractableHandler[] interactableObjects;
    public bool isTriggered; 

    
    void Update()
    {
        if (isTriggered)
        {
            for (int i = 0; i < interactableObjects.Length; i++)
            {
                InteractableHandler handler = interactableObjects[i];
                handler.PerformAction();
            }
        }

        isTriggered = false;
    }
}
