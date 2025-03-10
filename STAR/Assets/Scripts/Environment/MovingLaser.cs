using UnityEngine;

public class MovingLaser : MonoBehaviour
{
    public float moveSpeed;
    public float deadZone;
    public bool move;
    public float moveDistance;

    private float distanceMoved = 0f;

    void Update()
    {

        if (move)
        {
            distanceMoved += Time.deltaTime * moveSpeed;

            if (distanceMoved > moveDistance)
            {
                Destroy(gameObject);
            }
            transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime);
        }
    }
}
