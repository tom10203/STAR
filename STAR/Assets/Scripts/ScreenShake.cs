using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public bool start = false;
    public AnimationCurve curve;
    public float duration = 1f;

    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }

    }

    IEnumerator Shaking()
    {
        //Vector3 startPosition = transform.position;
        //float  elapsedTime = 0f;

        //while (elapsedTime < duration)
        //{
        //    elapsedTime += Time.deltaTime;
        //    float strength = curve.Evaluate(elapsedTime / duration);
        //    transform.position = startPosition + Random.insideUnitSphere * strength;
        //    yield return null;
        //}

        //transform.position = startPosition;

        float startFOV = Camera.main.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            Camera.main.fieldOfView = startFOV + Random.Range(-5,5) * strength;
            yield return null;
        }

        Camera.main.fieldOfView = startFOV;


    }
}
