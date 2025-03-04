using UnityEngine;

public struct CameraInput
{
    public Vector2 look;
}

public class PlayerCamera : MonoBehaviour
{
    private Vector3 _eulerAngles;

    [SerializeField] private float sensitivity = 0.1f;
    public void Initialise(Transform target)
    {
        transform.position = target.position;
        transform.eulerAngles = _eulerAngles = target.eulerAngles;
    }

    public void UpdatePosition(Transform target)
    {
        transform.position = target.position;
    }

    public void UpdateRotation(CameraInput input)
    {
        _eulerAngles += new Vector3(-input.look.y, input.look.x) * sensitivity;
        transform.eulerAngles = _eulerAngles;
    }
}
