using UnityEngine;

public abstract class InteractableHandler: MonoBehaviour
{

    public bool interact;
    public abstract void PerformAction();
    
}
