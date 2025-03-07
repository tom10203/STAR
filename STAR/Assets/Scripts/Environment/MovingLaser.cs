using UnityEngine;

public class MovingLaser : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public float deadZone;
    public bool move;

    void Update()
    {
        if (move)
        transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime);

        if (transform.localPosition.x < deadZone)
            Destroy(gameObject);
    }
}
