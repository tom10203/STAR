using System.Collections;
using UnityEngine;

public class RotatorInteraction : InteractableHandler
{
    [SerializeField] private float degreesToRotate = 180;
    [SerializeField] private float animationTime = 1;
    [SerializeField] private AudioSource hitSound;
    public override void PerformAction()
    {
        // This function is called once when asssociated TiggerObject script isTriggered is called
        StartCoroutine(Rotate());
        interact = false;

    }

    IEnumerator Rotate()
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 180f; // Rotate 180 degrees
        float timer = 0f;
        hitSound.Play();

        while (timer < 1)
        {
            timer += Time.deltaTime / animationTime;
            float newYRotation = Mathf.Lerp(startRotation, endRotation, timer);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newYRotation, transform.eulerAngles.z);
            yield return null;
        }

        // Ensure exact final rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, endRotation, transform.eulerAngles.z);
        interact = true;
    }

}
