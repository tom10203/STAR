using UnityEngine;

public class LaserWall : InteractableHandler
{
    MeshRenderer meshRenderer;
    BoxCollider boxCollider;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public override void PerformAction()
    {
        interact = false;
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
    }

}
