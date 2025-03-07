using UnityEngine;


public class TriggerObject : MonoBehaviour
{
    [SerializeField] private GameObject[] inActiveGameObjects; // adding this to be able to add gameObjects that start as inactive in order to call thier respective scripts

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
        ActivateGameObjects();

        for (int i = 0; i < interactableObjects.Length; i++)
        {
            InteractableHandler handler = interactableObjects[i];

            if (handler == null)
            {
                Debug.Log($"Handler == null");
            }
            if (handler.interact)
            handler.PerformAction();
        }
        isTriggered = false;
    }

    void ActivateGameObjects()
    {
        for (int i = 0; i < inActiveGameObjects.Length; i++)
        {
            GameObject gameObject = inActiveGameObjects[i];
            gameObject.SetActive(true);
        }
    }
}
