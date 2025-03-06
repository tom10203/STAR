using UnityEngine;

public abstract class InteractableHandler: MonoBehaviour
{

    public virtual bool interact { get; set; } = true;
    public virtual void PerformAction() { }
  
    
}
