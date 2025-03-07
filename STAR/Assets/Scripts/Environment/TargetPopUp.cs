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
        gameObject.SetActive(true);
        PopUp();
        interact = false;
    }

    void PopUp()
    {
        animator.SetTrigger("TargetPopUp");
    }
}
