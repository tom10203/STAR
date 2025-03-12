using System.Collections;
using UnityEngine;

public class LaserWall : InteractableHandler
{
    MeshRenderer meshRenderer;
    BoxCollider boxCollider;


    [SerializeField] private float interactionSpeed = 1;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public override void PerformAction()
    {
        interact = false;
        StartCoroutine(DissolveWall());

    }

    IEnumerator DissolveWall()
    {
        float cutoff = 0;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material material = meshRenderer.material;

        while (cutoff < 1.5)
        {
            cutoff += Time.deltaTime * interactionSpeed;
            material.SetFloat("_cutoff", cutoff);
            meshRenderer.material = material;
            yield return null;

        }
        boxCollider.enabled = false;
    }

}
