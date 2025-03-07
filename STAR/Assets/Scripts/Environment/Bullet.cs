using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;

    public Vector3 startPos;
    public Vector3 endPos;

    private Vector3 direction;
    private float distanceMoved;
    private float distanceToTravel;

    private bool destroyNextFrame;

    private void Start()
    {
        Vector3 dir = endPos - startPos;

        direction = dir.normalized;
        distanceToTravel = dir.magnitude;
    }

    void Update()
    {
        if (destroyNextFrame)
        {
            Destroy(gameObject);
        }


        distanceMoved += Time.deltaTime * bulletSpeed;

        if (distanceMoved > distanceToTravel) 
        {
            distanceMoved = distanceToTravel;
            destroyNextFrame = true;
        }

        transform.position = startPos + direction * distanceMoved;
    }
}
