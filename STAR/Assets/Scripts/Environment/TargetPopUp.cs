using UnityEngine;
using UnityEngine.UIElements;

public class TargetPopUp : InteractableHandler
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    public override void PerformAction()
    {
        PopUp();
        interact = false;
    }

    void PopUp()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();    
        }
        animator.SetTrigger("TargetPopUp");
    }
}
