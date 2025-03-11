using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class InGameButtonsTom : MonoBehaviour, IInteractable
{
    [SerializeField] private float interactionSpeed = 1;
    [SerializeField] GameObject[] interactionObjects;
    [SerializeField] private BoxCollider boxColliderToTurnOff;
    MeshRenderer m_Renderer;

    bool interact = true;
    public void Interact()
    {
        if (interact)
        {
            foreach (GameObject go in interactionObjects)
            {
                StartCoroutine(DissolveMaterial(go));
            }

            interact = false;
        }
    }

    IEnumerator DissolveMaterial(GameObject go)
    {
        float cutoff = 0f;

        MeshRenderer m_renderer = go.GetComponent<MeshRenderer>();
        Material material = m_renderer.material;

        if (!material.HasProperty("_cutoff"))
        {
            Debug.Log($"Material does NOT have cutoff property");
        }

        if (interactionSpeed <= 0)
        {
            interactionSpeed = 0.01f;
        }

        while (cutoff < 1.2)
        {
            cutoff += Time.deltaTime * interactionSpeed;
            material.SetFloat("_cutoff", cutoff);
            m_renderer.material = material;
            yield return null;
        }

        boxColliderToTurnOff.enabled = false;

        InGameUI inGameUI = FindAnyObjectByType<InGameUI>();
        inGameUI.StartTimer();

    }

}
