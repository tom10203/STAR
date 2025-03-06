using UnityEngine;


public class TriggerObject : MonoBehaviour
{
    [SerializeField] private InteractableHandler[] interactableObjects;
    [HideInInspector] public bool isTriggered;

    [SerializeField] private bool isTriggerCollider;
    void Update()
    {
        if (isTriggered)
        {
            StartInteractions();
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTriggerCollider)
        {
            if (other.gameObject.layer == 8)
            {
                StartInteractions();
            }
        }
    }

    void StartInteractions()
    {
        for (int i = 0; i < interactableObjects.Length; i++)
        {
            InteractableHandler handler = interactableObjects[i];
            if (handler.interact)
            handler.PerformAction();
        }
        isTriggered = false;
    }
}
